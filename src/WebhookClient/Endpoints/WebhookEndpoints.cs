using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace eShop.WebhookClient.Endpoints;

public static class WebhookEndpoints
{
    public static IEndpointRouteBuilder MapWebhookEndpoints(this IEndpointRouteBuilder app)
    {
        const string webhookCheckHeader = "X-eshop-whtoken";

        var configuration = app.ServiceProvider.GetRequiredService<IConfiguration>();
        bool.TryParse(configuration["ValidateToken"], out var validateToken);
        var tokenToValidate = configuration["WebhookClientOptions:Token"];

        app.MapMethods("/check", [HttpMethods.Options], Results<Ok, BadRequest<string>> ([FromHeader(Name = webhookCheckHeader)] string value, HttpResponse response) =>
        {
            if (!validateToken || value == tokenToValidate)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    response.Headers.Append(webhookCheckHeader, value);
                }

                return TypedResults.Ok();
            }

            return TypedResults.BadRequest("Invalid token");
        });

        app.MapPost("/webhook-received", async (WebhookData hook, HttpRequest request, ILogger<Program> logger, HooksRepository hooksRepository) =>
        {
            var token = request.Headers[webhookCheckHeader];

            logger.LogInformation("Received hook with token {Token}. My token is {MyToken}. Token validation is set to {ValidateToken}", token, tokenToValidate, validateToken);

            if (!validateToken || tokenToValidate == token)
            {
                logger.LogInformation("Received hook is going to be processed");
                var newHook = new WebHookReceived()
                {
                    Data = hook.Payload,
                    When = hook.When,
                    Token = token
                };
                await hooksRepository.AddNew(newHook);
                logger.LogInformation("Received hook was processed.");
                return Results.Ok(newHook);
            }

            logger.LogInformation("Received hook is NOT processed - Bad Request returned.");
            return Results.BadRequest();
        });



        // REVIEW: 11.11.2024 Disabled for now


        //app.MapPost("/webhook-cj-listener", async (HttpContext context) =>
        //{
        //    var clientIp = context.Connection.RemoteIpAddress?.ToString();
        //    Console.WriteLine("Client IP: " + clientIp);
        //    Console.WriteLine(context.Request.Path);

        //    // Read the request body
        //    string requestBody;
        //    using (StreamReader reader = new StreamReader(context.Request.Body))
        //    {
        //        requestBody = await reader.ReadToEndAsync();
        //    }
        //    // Log the raw request body
        //    Console.WriteLine("Raw request body: " + requestBody);

        //    // Additional processing can go here

        //    return Results.Ok(true); // Ensure to return a response
        //});





        return app;
    }
}

public class CallbackParams { 
    public string? MessageId { get; set; } 
    public string? Type { get; set; }
    public object? Params { get; set; }
}
