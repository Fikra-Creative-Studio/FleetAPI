﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Enums;
using Fleet.Repository;
using Fleet.Service;
using System.Text;
using Fleet.Mapper;

namespace Fleet.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// **** Injeção de dependência ****
        ///Services AddSingleton sempre que o controller for instanciado, meu objeto será instanciado até a finalização da aplicação para TODOS os usuários da aplicação
        ///Services AddScoped objeto instanciado até a finalização do método 
        ///Services AddAddTransient  toda vez que o controlador é instanciado, será gerado uma nova instância do objeto em dependencia
        /// </summary>
        /// <param name="services"></param>
        public static void AdicionarDependenciasFleet(this IServiceCollection services, IConfiguration configuration)
        {
            services.AdicionarMySQL(configuration);
            services.AdicionarDependenciasRepositorio();
            services.AdicionarDependenciasServicos();
            services.AdicionarAutenticacao(configuration);
            services.AdicionarAutorizacao();
            services.AdicionarMapper();
        }

        /// <summary>
        /// Injeção de dependências para acesso aos serviços / regras de negócio
        /// </summary>
        /// <param name="services"></param>
        private static void AdicionarDependenciasServicos(this IServiceCollection services)
        {
            services.AddScoped<ILoggedUser, LoggedUser>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IBucketService, FileStorageService>();
            services.AddScoped<IWorskpaceService, WorkspaceService>();
            services.AddScoped<IVeiculoService, VeiculoService>();
            services.AddScoped<IEstabelecimentoService, EstabelecimentoService>();
            services.AddScoped<IListaService, ListaService>();
            services.AddScoped<IListaItemService, ListaItemService>();
            services.AddScoped<IAbastecimentoService, AbastecimentoService>();
            services.AddScoped<IManutencaoService, ManutencaoService>();
            services.AddScoped<ICheckListService, CheckListService>();
            services.AddScoped<IVisitaService, VisitaService>();
            services.AddScoped<IRelatorioVisitaService, RelatorioVisitaService>();
        }

        /// <summary>
        /// Injeção de dependencias para acesso ao repositório
        /// </summary>
        /// <param name="services"></param>
        private static void AdicionarDependenciasRepositorio(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
            services.AddScoped<IUsuarioWorkspaceRepository, UsuarioWorkspaceRepository>();
            services.AddScoped<IVeiculoRepository, VeiculoRepository>();
            services.AddScoped<IEstabelecimentoRepository, EstabelecimentoRepository>();
            services.AddScoped<IListaRepository, ListaRepository>();
            services.AddScoped<IListaItemRepository, ListaItemRepository>();
            services.AddScoped<IAbastecimentoRepository, AbastecimentoRepository>();
            services.AddScoped<IManutencaoRepository, ManutencaoRepository>();
            services.AddScoped<ICheckListRepository, CheckListRepository>();
            services.AddScoped<IVisitaRepository, VisitaRepository>();
            services.AddScoped<IRelatorioVisitaRepository, RelatorioVisitaRepository>();
        }

        private static void AdicionarMySQL(this IServiceCollection services, IConfiguration configuration)
        {
            string? mySqlConnection = configuration.GetConnectionString("DB_MySQL");  //Endereço do banco de dados
            services.AddDbContextPool<ApplicationDbContext>(options => options.UseMySql(mySqlConnection, ServerVersion.Parse("5.7.32")));
        }

        private static void AdicionarAutenticacao(this IServiceCollection services, IConfiguration configuration)
        {
            var secret = configuration.GetSection("Authorization").GetValue<string>("Secret");

            var key = Encoding.ASCII.GetBytes(secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        private static void AdicionarAutorizacao(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("user", policy => policy.RequireClaim("papel", PapelEnum.Usuario.ToString()));
                options.AddPolicy("admin", policy => policy.RequireClaim("papel", PapelEnum.Administrador.ToString()));
            });
        }

        private static void AdicionarMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Mapping));
        }
    }
}