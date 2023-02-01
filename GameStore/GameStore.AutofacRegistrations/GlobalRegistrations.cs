using System.Reflection;
using Autofac;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.Services;
using GameStore.BusinessLogicLayer.Abstract.Auth;
using GameStore.BusinessLogicLayer.Abstract.Infrastructure;
using GameStore.BusinessLogicLayer.Services.Auth;
using GameStore.BusinessLogicLayer.Services.Filter;
using GameStore.BusinessLogicLayer.Services.Payment;
using GameStore.BusinessLogicLayer.Abstract.Payment;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.BusinessLogicLayer.Services.Payment.Strategies;
using GameStore.Domain.Abstract;
using GameStore.Domain.EF;
using GameStore.Domain.Entities;
using GameStore.Domain.Mongo;
using GameStore.Domain.Mongo.Abstract;
using GameStore.Domain.Mongo.Entities;
using GameStore.Domain.Mongo.Repositories;
using GameStore.Domain.Repositories;
using MongoOrder = GameStore.Domain.Mongo.Entities.Order;
using Order = GameStore.Domain.Entities.Order;

namespace GameStore.AutofacRegistrations
{
    public class GlobalRegistrations
    {
        public static ContainerBuilder ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<MongoUnitOfWork>().As<IMongoUnitOfWork>();
            builder.RegisterType<GameStoreContext>().As<IDbContext>()
                .WithParameter("connection", "DefaultConnection")
                .InstancePerRequest();
            builder.RegisterType<GameStoreMongoContext>().As<IMongoContext>()
                .WithParameter("connectionString", "MongoDb")
                .InstancePerRequest();
            builder.RegisterType<GameRepository>().As<IRepository<Game>>().InstancePerRequest();
            builder.RegisterType<AbstractLoggingRepository<Genre>>().As<IRepository<Genre>>().InstancePerRequest();
            builder.RegisterType<AbstractLoggingRepository<Comment>>().As<IRepository<Comment>>().InstancePerRequest();
            builder.RegisterType<AbstractLoggingRepository<PlatformType>>().As<IRepository<PlatformType>>()
                .InstancePerRequest();
            builder.RegisterType<AbstractLoggingRepository<Publisher>>().As<IRepository<Publisher>>().InstancePerRequest();
            builder.RegisterType<AbstractLoggingRepository<Order>>().As<IRepository<Order>>().InstancePerRequest();
            builder.RegisterType<AbstractLoggingRepository<OrderDetails>>().As<IRepository<OrderDetails>>()
                .InstancePerRequest();
            builder.RegisterType<AbstractLoggingRepository<Role>>().As<IRepository<Role>>().InstancePerRequest();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();
            builder.RegisterType<LogRepository>().As<ILogRepository>().InstancePerRequest();
            builder.RegisterType<MongoAbstractRepository<MongoOrder>>().As<IMongoRepository<MongoOrder>>()
                .WithParameter("collectionName", "orders")
                .InstancePerRequest();
            builder.RegisterType<MongoAbstractRepository<Shipper>>().As<IMongoRepository<Shipper>>()
                .WithParameter("collectionName", "shippers")
                .InstancePerRequest();
            builder.RegisterType<GameService>().As<IGameService>();
            builder.RegisterType<CommentService>().As<ICommentService>();
            builder.RegisterType<GenreService>().As<IGenreService>();
            builder.RegisterType<PlatformTypeService>().As<IPlatformTypeService>();
            builder.RegisterType<PublisherService>().As<IPublisherService>();
            builder.RegisterType<OrderService>().As<IOrderService>();
            builder.RegisterType<BankPaymentStrategy>().Keyed<IPaymentStrategy>(PaymentType.Bank);
            builder.RegisterType<IBoxPaymentStrategy>().Keyed<IPaymentStrategy>(PaymentType.IBox);
            builder.RegisterType<VisaPaymentStrategy>().Keyed<IPaymentStrategy>(PaymentType.Visa);
            builder.RegisterType<PaymentService>().As<IPaymentService>();
            builder.RegisterType<CommentService>().As<ICommentService>();
            builder.RegisterType<ShipperService>().As<IShipperService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<RoleService>().As<IRoleService>();
            builder.RegisterType<HashPasswordManager>().As<IHashPasswordManager>();
            builder.RegisterType<GameSelectionPipeline>().AsSelf();
            builder.RegisterType<AuthenticationService>().As<IAuthentication>().InstancePerRequest();
            builder.RegisterType<UserIndentity>().AsSelf();
            builder.RegisterType<UserProvider>().AsSelf();
            return builder;
        }
    }
}