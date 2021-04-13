using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using CNX.Core.Extensions.JWT;
using System;
using System.IO;
using System.Reflection;

namespace CNX.Core.WebApi
{


        public class AutofacModuleRegister : Autofac.Module
        {
           // private static readonly ILog log = LogManager.GetLogger(typeof(AutofacModuleRegister));
            protected override void Load(ContainerBuilder builder)
            {
                var basePath = AppContext.BaseDirectory;
                //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();


                #region 带有接口层的服务注入

                var servicesDllFile = Path.Combine(basePath, "CNX.Core.Services.dll");
                var repositoryDllFile = Path.Combine(basePath, "CNX.Core.Repository.dll");

                if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
                {
                    var msg = "Repository.dll和service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                   // log.Error(msg);
                    throw new Exception(msg);
                }

                //builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();//注册仓储

                // 获取 Service.dll 程序集服务，并注册
                var assemblysServices = Assembly.LoadFrom(servicesDllFile);
                builder.RegisterAssemblyTypes(assemblysServices)
                          .AsImplementedInterfaces()
                          .InstancePerDependency();
                          //.EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                          //.InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册。

                // 获取 Repository.dll 程序集服务，并注册
                var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
                builder.RegisterAssemblyTypes(assemblysRepository)
                       .AsImplementedInterfaces()
                       .InstancePerDependency();

            #endregion

            #region 没有接口的单独类，启用class代理拦截

            //只能注入该类中的虚方法，且必须是public
            //这里仅仅是一个单独类无接口测试，不用过多追问
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(JWTTokenBuilder)))
                .EnableClassInterceptors();
                //.InterceptedBy(cacheType.ToArray()
                                              
            #endregion

        }
    }





    }