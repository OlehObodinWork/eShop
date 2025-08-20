using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Pgvector.EntityFrameworkCore;


namespace eShop.Catalog.API;

public static class PrimaryCatalogApi
{
    public static IEndpointRouteBuilder MapCatalogPrimaryApiV1(this IEndpointRouteBuilder app)
    {
        
        var vApi = app.NewVersionedApi("PrimaryCatalog");
        var api = vApi.MapGroup("api/primary-catalog").HasApiVersion(1, 0).HasApiVersion(2, 0);
        var v1 = vApi.MapGroup("api/primary-catalog").HasApiVersion(1, 0);
        var v2 = vApi.MapGroup("api/primary-catalog").HasApiVersion(2, 0);


        // Routes for querying catalog items.
        api.MapGet("/items", GetAllItems);
        api.MapGet("/items/by", GetItemsByIds);
        api.MapGet("/items/{id:int}", GetItemById);
        api.MapGet("/items/by/{name:minlength(1)}", GetItemsByName);
        api.MapGet("/items/{catalogItemId:int}/pic", GetItemPictureById);

        // Routes for resolving catalog items using AI.
        api.MapGet("/items/withsemanticrelevance/{text:minlength(1)}", GetItemsBySemanticRelevance);

        // Routes for resolving catalog items by type and brand.
        api.MapGet("/items/type/{typeId}/brand/{brandId?}", GetItemsByBrandAndTypeId);
        api.MapGet("/items/type/all/brand/{brandId:int?}", GetItemsByBrandId);
        //api.MapGet("/catalogtypes", async (CatalogContext context) => await context.CatalogTypes.OrderBy(x => x.Type).ToListAsync());
        //api.MapGet("/catalogbrands", async (CatalogContext context) => await context.CatalogBrands.OrderBy(x => x.Brand).ToListAsync());

        // Routes for modifying catalog items.
        api.MapPut("/items", UpdateItem);
        api.MapPost("/items", CreateItem);

        api.MapDelete("/items/{id:int}", DeleteItemById);
        api.MapGet("items/sync", SyncItem);


        api.MapPost("/features", CreateFeature);
        api.MapGet("/features", GetAllFeatures);
        api.MapPost("/catalog-features-values/{catalogId:int?}", AddFeatureToCatalogItem);
        api.MapGet("/catalog-features-values/{catalogId:int?}", GetFeaturesByCatalogId);
        return app;
    }

    public static async Task<Results<Ok<PaginatedItems<PrimaryCatalogItem>>, BadRequest<string>>> GetAllItems(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var totalItems = await services.Context.PrimaryCatalogItems
            .LongCountAsync();

        var itemsOnPage = await services.Context.PrimaryCatalogItems
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<PrimaryCatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Ok<List<PrimaryCatalogItem>>> GetItemsByIds(
        [AsParameters] CatalogServices services,
        int[] ids)
    {
        var items = await services.Context.PrimaryCatalogItems.Where(item => ids.Contains(item.Id)).ToListAsync();
        return TypedResults.Ok(items);
    }

    public static async Task<Results<Ok<PrimaryCatalogItem>, NotFound, BadRequest<string>>> GetItemById(
        [AsParameters] CatalogServices services,
        int id)
    {
        if (id <= 0)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        var item = await services.Context.PrimaryCatalogItems.Include((ci) => ci.PrimaryCatalogItemVariants).SingleOrDefaultAsync(ci => ci.Id == id);
        
        if (item == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(item);
    }

    public static async Task<Ok<PaginatedItems<PrimaryCatalogItem>>> GetItemsByName(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services,
        string name)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var totalItems = await services.Context.PrimaryCatalogItems
            .Where(c => c.Name.StartsWith(name))
            .LongCountAsync();

        var itemsOnPage = await services.Context.PrimaryCatalogItems
            .Where(c => c.Name.StartsWith(name))
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<PrimaryCatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Results<NotFound, PhysicalFileHttpResult>> GetItemPictureById(CatalogContext context, IWebHostEnvironment environment, int catalogItemId)
    {
        var item = await context.PrimaryCatalogItems.FindAsync(catalogItemId);

        if (item is null)
        {
            return TypedResults.NotFound();
        }

        var path = GetFullPath(environment.ContentRootPath, item.PictureFileName);

        string imageFileExtension = Path.GetExtension(item.PictureFileName);
        string mimetype = GetImageMimeTypeFromImageFileExtension(imageFileExtension);
        DateTime lastModified = File.GetLastWriteTimeUtc(path);

        return TypedResults.PhysicalFile(path, mimetype, lastModified: lastModified);
    }

    public static async Task<Results<BadRequest<string>, RedirectToRouteHttpResult, Ok<PaginatedItems<PrimaryCatalogItem>>>> GetItemsBySemanticRelevance(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services,
        string text)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        if (!services.PrimaryCatalogAI.IsEnabled)
        {
            return await GetItemsByName(paginationRequest, services, text);
        }

        // Create an embedding for the input search
        var vector = await services.PrimaryCatalogAI.GetEmbeddingAsync(text);

        // Get the total number of items
        var totalItems = await services.Context.CatalogItems
            .LongCountAsync();

        // Get the next page of items, ordered by most similar (smallest distance) to the input search
        List<PrimaryCatalogItem> itemsOnPage;
        if (services.Logger.IsEnabled(LogLevel.Debug))
        {
            var itemsWithDistance = await services.Context.PrimaryCatalogItems
                .Select(c => new { Item = c, Distance = c.Embedding.CosineDistance(vector) })
                .OrderBy(c => c.Distance)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            services.Logger.LogDebug("Results from {text}: {results}", text, string.Join(", ", itemsWithDistance.Select(i => $"{i.Item.Name} => {i.Distance}")));

            itemsOnPage = itemsWithDistance.Select(i => i.Item).ToList();
        }
        else
        {
            itemsOnPage = await services.Context.PrimaryCatalogItems
                .OrderBy(c => c.Embedding.CosineDistance(vector))
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();
        }

        return TypedResults.Ok(new PaginatedItems<PrimaryCatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Ok<PaginatedItems<PrimaryCatalogItem>>> GetItemsByBrandAndTypeId(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services,
        int typeId,
        int? brandId)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var root = (IQueryable<PrimaryCatalogItem>)services.Context.PrimaryCatalogItems;
        root = root.Where(c => c.CatalogTypeId == typeId);
        if (brandId is not null)
        {
            root = root.Where(c => c.CatalogBrandId == brandId);
        }

        var totalItems = await root
            .LongCountAsync();

        var itemsOnPage = await root
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<PrimaryCatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Ok<PaginatedItems<PrimaryCatalogItem>>> GetItemsByBrandId(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services,
        int? brandId)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var root = (IQueryable<PrimaryCatalogItem>)services.Context.CatalogItems;

        if (brandId is not null)
        {
            root = root.Where(ci => ci.CatalogBrandId == brandId);
        }

        var totalItems = await root
            .LongCountAsync();

        var itemsOnPage = await root
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<PrimaryCatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    [Authorize(Roles = "Admin")]

    public static async Task<Results<Created, NotFound<string>>> UpdateItem(
        [AsParameters] CatalogServices services,
        PrimaryCatalogItem productToUpdate)
    {
        var catalogItem = await services.Context.PrimaryCatalogItems.SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);

        if (catalogItem == null)
        {
            return TypedResults.NotFound($"Item with id {productToUpdate.Id} not found.");
        }

        // Update current product
        var catalogEntry = services.Context.Entry(catalogItem);
        catalogEntry.CurrentValues.SetValues(productToUpdate);

        catalogItem.Embedding = await services.PrimaryCatalogAI.GetEmbeddingAsync(catalogItem);

        var priceEntry = catalogEntry.Property(i => i.Price);

        if (priceEntry.IsModified) // Save product's data and publish integration event through the Event Bus if price has changed
        {
            //Create Integration Event to be published through the Event Bus
            var priceChangedEvent = new ProductPriceChangedIntegrationEvent(catalogItem.Id, productToUpdate.Price, priceEntry.OriginalValue);

            // Achieving atomicity between original Catalog database operation and the IntegrationEventLog thanks to a local transaction
            await services.EventService.SaveEventAndCatalogContextChangesAsync(priceChangedEvent);

            // Publish through the Event Bus and mark the saved event as published
            await services.EventService.PublishThroughEventBusAsync(priceChangedEvent);
        }
        else // Just save the updated product because the Product's Price hasn't changed.
        {
            await services.Context.SaveChangesAsync();
        }
        return TypedResults.Created($"/api/primary-catalog/items/{productToUpdate.Id}");
    }
    [Authorize(Roles = "Admin")]

    public static async Task<Created> CreateItem(
        [AsParameters] CatalogServices services,
        PrimaryCatalogItem product)
    {
        var item = new PrimaryCatalogItem
        {
            Id = product.Id,
            CatalogBrandId = product.CatalogBrandId,
            CatalogTypeId = product.CatalogTypeId,
            Description = product.Description,
            Name = product.Name,
            PictureFileName = product.PictureFileName,
            Price = product.Price,
            AvailableStock = product.AvailableStock,
            RestockThreshold = product.RestockThreshold,
            MaxStockThreshold = product.MaxStockThreshold
        };
        item.Embedding = await services.PrimaryCatalogAI.GetEmbeddingAsync(item);

        services.Context.PrimaryCatalogItems.Add(item);
        await services.Context.SaveChangesAsync();

        return TypedResults.Created($"/api/primary-catalog/items/{item.Id}");
    }
    [Authorize(Roles = "Admin")]

    public static async Task<Results<NoContent, NotFound>> DeleteItemById(
        [AsParameters] CatalogServices services,
        int id)
    {
        var item = services.Context.PrimaryCatalogItems.SingleOrDefault(x => x.Id == id);

        if (item is null)
        {
            return TypedResults.NotFound();
        }

        services.Context.PrimaryCatalogItems.Remove(item);
        await services.Context.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    [Authorize(Roles = "Admin")]

    public static async Task<Results<Created, Ok>> SyncItem([AsParameters] CatalogServices services, string SKU)
    {
        var url = $"https://developers.cjdropshipping.com/api2.0/v1/product/query?productSku={SKU}";

        var token = "";
        if (services.TokenService.token != null)
        {
            token = services.TokenService.token;
        }
        else
        {
            token = await services.TokenService.GetTokenAsync();
        }

        services.Logger.LogInformation(token);

        var result = await services.CJCatalog.GetCatalogItemAsync( url, services.Logger, token);
        var item = services.Context.PrimaryCatalogItems.SingleOrDefault(x => x.ProductSKU == SKU);


        //Type type = result.Item.GetType();
        //PropertyInfo[] properties = type.GetProperties();

        //foreach (var property in properties)
        //{
        //    var value = property.GetValue(result.Item);
        //    services.Logger.LogInformation("{PropertyName}: {PropertyValue}", property.Name, value);
        //}

        if (item != default && item.ListedNum != result.Item.ListedNum)
        {
            item.ListedNum = result.Item.ListedNum;
            await services.Context.SaveChangesAsync();
            return TypedResults.Ok();
        }
        if (item == default)
        {
            foreach (var variant in result.Variants)
            {
                variant.PrimaryCatalogItemId = result.Item.Id;
                variant.VarianPriceAdjustments();
                variant.VarianKeyAdjusted();
                result.Item.PrimaryCatalogItemVariants.Add(variant);
                services.Context.PrimaryCatalogItemVariants.Add(variant);
            }

            foreach (var image in result.Images)
            {
                image.PrimaryCatalogItemId = result.Item.Id;
                result.Item.PrimaryCatalogOriginalImages.Add(image);
                services.Context.PrimaryCatalogOriginalImages.Add(image);
            }


            // var item = new CatalogItem
            //{
            //    Id = product.Id,
            //    CatalogBrandId = product.CatalogBrandId,
            //    CatalogTypeId = product.CatalogTypeId,
            //    Description = product.Description,
            //    Name = product.Name,
            //    PictureFileName = product.PictureFileName,
            //    Price = product.Price,
            //    AvailableStock = product.AvailableStock,
            //    RestockThreshold = product.RestockThreshold,
            //    MaxStockThreshold = product.MaxStockThreshold
            //};
            result.Item.Embedding = await services.PrimaryCatalogAI.GetEmbeddingAsync(item);

            services.Context.PrimaryCatalogItems.Add(result.Item);
            await services.Context.SaveChangesAsync();
            return TypedResults.Created($"/api/primary-catalog/{result.Item.Id}");
        }
        return TypedResults.Ok();
    }


    private static string GetImageMimeTypeFromImageFileExtension(string extension) => extension switch
    {
        ".png" => "image/png",
        ".gif" => "image/gif",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".bmp" => "image/bmp",
        ".tiff" => "image/tiff",
        ".wmf" => "image/wmf",
        ".jp2" => "image/jp2",
        ".svg" => "image/svg+xml",
        ".webp" => "image/webp",
        _ => "application/octet-stream",
    };

    public static string GetFullPath(string contentRootPath, string pictureFileName) =>
        Path.Combine(contentRootPath, "Pics", pictureFileName);

    [Authorize(Roles = "Admin")]

    public static async Task<Created> CreateFeature(
        [AsParameters] CatalogServices services,
        PrimaryCatalogFeature feature)
    {

        // Don't need any validation for now

        var newFeature = new PrimaryCatalogFeature
        {
            Title = feature.Title,
            Icon = feature.Icon,
        };

        services.Context.PrimaryCatalogFeatures.Add(newFeature);
        await services.Context.SaveChangesAsync();

        return TypedResults.Created($"/api/primary-catalog/features/{feature.Id}");
    }

    [Authorize(Roles = "Admin")]

    public static async Task<Results<Created, NotFound<string>>> UpdateFeatureValue(
       [AsParameters] CatalogServices services,
       PrimaryCatalogFeatureValue featureValueToUpdate)
    {
        var catalogFeatureValue = await services.Context.PrimaryCatalogFeatureValues.SingleOrDefaultAsync(i => i.Id == featureValueToUpdate.Id);

        if (catalogFeatureValue == null)
        {
            return TypedResults.NotFound($"Item with id {catalogFeatureValue.Id} not found.");
        }

        // Update current product
        var featureValueEntry = services.Context.Entry(catalogFeatureValue);
        featureValueEntry.CurrentValues.SetValues(catalogFeatureValue);


        await services.Context.SaveChangesAsync();
        return TypedResults.Created($"/api/primary-catalog/catalog-features-values/{catalogFeatureValue.Id}");
    }

    [Authorize(Roles = "Admin")]

    public static async Task<Results<Ok<PaginatedItems<PrimaryCatalogFeature>>, BadRequest<string>>> GetAllFeatures(
    [AsParameters] PaginationRequest paginationRequest,
    [AsParameters] CatalogServices services)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var totalItems = await services.Context.PrimaryCatalogFeatures
            .LongCountAsync();

        var featuresOnPage = await services.Context.PrimaryCatalogFeatures
            .OrderBy(c => c.Title)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<PrimaryCatalogFeature>(pageIndex, pageSize, totalItems, featuresOnPage));
    }

    [Authorize(Roles = "Admin")]

    public static async Task<Created> AddFeatureToCatalogItem([AsParameters] CatalogServices services, PrimaryCatalogFeatureValue catalogFeatureValue)
    {
        services.Context.PrimaryCatalogFeatureValues.Add(catalogFeatureValue);
        await services.Context.SaveChangesAsync();
        return TypedResults.Created($"/api/primary-catalog/catalog-features-values/{catalogFeatureValue.Id}");
    }

    public static async Task<Ok> ChangeFeatureValueCatalogItem([AsParameters] CatalogServices services, PrimaryCatalogFeatureValue catalogFeatureValue)
    {
        //var newCatalogFeatureValue = new CatalogFeatureValues
        //{
        //    CatalogFeatureId = catalogFeatureValue.CatalogFeatureId,
        //    CatalogItemId = catalogFeatureValue.FeatureName,
        //}
        services.Context.PrimaryCatalogFeatureValues.Add(catalogFeatureValue);
        await services.Context.SaveChangesAsync();
        return TypedResults.Ok();
    }

    public static async Task<Results<Ok<List<PrimaryCatalogFeatureValue>>, NotFound, BadRequest<string>>> GetFeaturesByCatalogId(
        [AsParameters] CatalogServices services,
        int id)
    {
        var features = await services.Context.PrimaryCatalogFeatureValues
            .Where(cf => id == cf.PrimaryCatalogItemId).ToListAsync();

        if (features == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(features);
    }

}
