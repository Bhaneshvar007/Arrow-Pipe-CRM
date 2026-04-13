using Microsoft.Extensions.FileProviders;
using SalesCrm.CommonHelpers;
using SalesCrm.Services.DAL;
using SalesCrm.Services.IDAL;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173",
            "https://arrowpipecrm.cylsysuat.com")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

//DI
builder.Services.AddScoped<IDALClientConfigurationForm, DALClientConfigurationForm>();
builder.Services.AddScoped<IDALClientPOCConfigurationForm, DALClientPOCConfigurationForm>();
builder.Services.AddScoped<IDALClient, DALClient>();
builder.Services.AddScoped<IDALClientPOC, DALClientPOC>();
builder.Services.AddScoped<IDALAttachmentDetails, DALAttachmentDetails>();
builder.Services.AddScoped<IDALDropDownMaster, DALDropDownMaster>();
builder.Services.AddScoped<IDALDropDownCategory, DALDropDownCategory>();
builder.Services.AddScoped<IDALLeadConfiguration, DALLeadConfiguration>();
builder.Services.AddScoped<IDALLeadData, DALLeadData>();
builder.Services.AddScoped<IDALLeads, DALLeads>();
builder.Services.AddScoped<IDALEmailConfigurations, DALEmailConfigurations>();
builder.Services.AddScoped<IDAlSKUMaster, DALSKUMaster>();
builder.Services.AddScoped<IDALMailWorkflow, DALMailWorkflow>();
builder.Services.AddScoped<IDALAIGeneratedSuit, DALAIGeneratedSuit>();



builder.Services.AddJWTAuthentication(builder.Configuration);




CommonHelper.Init(builder.Configuration);

var app = builder.Build();



app.UseCors("AllowAll");  

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        config["FileUploads:FilePath"]
    ),
    RequestPath = "/FileUploads",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "*");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Methods", "*");
    }
});


//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sales CRM API");
    c.RoutePrefix = string.Empty; // This makes Swagger load on root URL
});


app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
