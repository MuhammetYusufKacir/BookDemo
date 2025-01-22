using Autofac;
using BookDemo.Application.Services;
using BookDemo.Core.Interfaces;
using BookDemo.Infrastructure;
using BookDemo.Infrastructure.Repositories;

namespace BookDemoAPI.Autofac
{
    public static class AutofacBook
    {
        public static ContainerBuilder Load(this ContainerBuilder builder)
        {
            builder.RegisterType<BookService>().As<IBookService>().InstancePerLifetimeScope();
            builder.RegisterType<BookRepository>().As<IBookRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SqlManager>().As<ISqlManager>().SingleInstance();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
            return builder;
        }
    }
}
