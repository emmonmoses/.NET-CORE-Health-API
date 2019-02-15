using HealthAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace HealthAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HealthContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Add Cors
            services.AddCors(o => o.AddPolicy("HealthPolicy", builder => {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v4", new Info
                {
                    Version = "v1",
                    Title = "HealthAPI",
                    Description = "My simple example of an ASP.NET Core Web APi conected to SQLSERVER",
                    TermsOfService = "https://drive.google.com/open?id=1ZLQgzoJP37Odr2hB5A3svGoNxOUY4y3x",
                    Contact = new Contact
                    {
                        Name = "Developer",
                        Email = "oemmon@yahoo.com",
                        Url = "https://github.com/emmonmoses"
                    },
                    License = new License
                    {
                        Name = " If I believe you or Your Users are " +
                               "using my tools in any way that undermines my personal interests, I may, at  sole " +
                               "discretion, terminate these Terms, suspend your license to use the API, discontinue your participation in " +
                               "this Program, terminate your access to my tools, and / or reduce your access to all or some " +
                               " APIs",
                        Url = "https://drive.google.com/open?id=1ZLQgzoJP37Odr2hB5A3svGoNxOUY4y3x"
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v4/swagger.json", "Core API");
            });
            DummyData.Initialize(app);
        }
    }
}
