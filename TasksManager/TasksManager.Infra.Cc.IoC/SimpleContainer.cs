using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using TasksManager.Application.Apps;
using TasksManager.Application.Interfaces;
using TasksManager.Domain.Interfaces.Repositories;
using TasksManager.Domain.Interfaces.Services;
using TasksManager.Domain.Services;
using TasksManager.Infra.Data.Context;
using TasksManager.Infra.Data.Repositories;

namespace TasksManager.Infra.Cc.IoC
{
    public class SimpleContainer
    {
        public Container Initialize()
        {
            var container = new Container();

            var scopedLifestyle = new WebApiRequestLifestyle();
            container.Options.DefaultScopedLifestyle = scopedLifestyle;

            //Context
            container.Register<MongoDbContext>(Lifestyle.Scoped);

            //MongoDb
            container.Register(typeof(IBaseRepository<>), typeof(BaseRepository<>), Lifestyle.Scoped);
            container.Register<ITaskRepository, TaskRepository>(Lifestyle.Scoped);

            //Services Domain
            container.Register<ITaskService, TaskService>(Lifestyle.Scoped);

            //App
            container.Register<IUserApp>(() => new UserApp(new TaskUserService(new TaskUserRepository(new MongoDbContext()))), Lifestyle.Transient);
            container.Register<ITaskApp, TaskApp>(Lifestyle.Scoped);


            return container;

        }
    }
}
