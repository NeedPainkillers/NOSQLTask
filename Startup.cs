using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NOSQLTask.Data;
using NOSQLTask.Repository;

namespace NOSQLTask
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //    builder => builder.AllowAnyOrigin()
            //                      .AllowAnyMethod()
            //                      .AllowAnyHeader()
            //                      .AllowCredentials());
            //});

            services.Configure<Settings>(options =>
            {
                options.MongoConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.MongoDatabase = Configuration.GetSection("MongoConnection:Database").Value;

                options.PostgresConnectionString = Configuration.GetSection("PostgresConnection:ConnectionString").Value;

                options.RedisConnectionString = Configuration.GetSection("RedisConnection:ConnectionString").Value;

                options.Neo4jConnectionUrl = Configuration.GetSection("Neo4jConnection:ConnectionString").Value;
                options.Neo4jConnectionLogin = Configuration.GetSection("Neo4jConnection:User").Value;
                options.Neo4jConnectionPassword = Configuration.GetSection("Neo4jConnection:Password").Value;

                options.ElascticConnectionString = Configuration.GetSection("ESConnection:ConnectionString").Value;
            });

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSingleton<IInvoiceRepository, InvoiceRepository>();
            services.AddSingleton<IClientRepository, ClientRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<INeo4jRepository, Neo4jRepository>();
            services.AddSingleton<IVisitLogRepository, VisitLogRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
