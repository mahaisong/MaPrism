using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

#nullable enable
namespace Prism.Container.DryIoc
{

    internal sealed class DryIocServiceScopeFactory : IServiceScopeFactory
    {
         
        private readonly IResolverContext _scopedResolver;

        public DryIocServiceScopeFactory(IResolverContext scopedResolver)
        {
            this._scopedResolver = scopedResolver;
        }

        public IServiceScope CreateScope()
        {
            IResolverContext scopedResolver = this._scopedResolver;



            IScope iscope = scopedResolver.ScopeContext == null ?
                Scope.Of(scopedResolver.OwnCurrentScope)
                :
                 //SetCurrent方法里面是委托--》这里面是SetCurrentScopeHandler setCurrentScope 委托。

                 //模仿写法
                 //p => Scope.Of(p)

                 scopedResolver.ScopeContext.SetCurrent(p => Scope.Of(p));


            return (IServiceScope)new DryIocServiceScope(scopedResolver.WithCurrentScope(iscope));
        }
    }

    //   internal sealed class DryIocServiceScopeFactory : IServiceScopeFactory
    //   {

    //       private sealed class a
    //{
    //	public static readonly a c = new a();

    //       public static SetCurrentScopeHandler c__2_0;

    //	internal IScope<CreateScope> b__2_0(IScope p)
    //       {
    //           return Scope.Of(p);
    //       }
    //}


    //       private readonly IResolverContext _scopedResolver;

    //       public DryIocServiceScopeFactory(IResolverContext scopedResolver)
    //       {
    //           this._scopedResolver = scopedResolver;
    //       }

    //       public IServiceScope CreateScope()
    //       {
    //           IResolverContext scopedResolver = this._scopedResolver;



    //           IScope iscope = scopedResolver.ScopeContext == null ?
    //               Scope.Of(scopedResolver.OwnCurrentScope)
    //               :
    //                //SetCurrent方法里面是委托--》这里面是SetCurrentScopeHandler setCurrentScope 委托。

    //                //模仿写法
    //                //p => Scope.Of(p)

    //                scopedResolver.ScopeContext.SetCurrent(p => Scope.Of(p));

    //               scopedResolver.ScopeContext.SetCurrent(

    //                   //内部类+内部类的对象(SetCurrentScopeHandler委托的默认方法)+实现其实是b__2_0--参数是IScope<CreateScope>
    //                   DryIocServiceScopeFactory.a.c__2_0 ??
    //                   (
    //                   DryIocServiceScopeFactory.a.c__2_0
    //                   = new SetCurrentScopeHandler(
    //                       (object)DryIocServiceScopeFactory.a.c,
    //                       //匿名方法-lamada表达式
    //                       __methodptr(<CreateScope>b__2_0)
    //                       )
    //                   )
    //               )
    //               );


    //           return (IServiceScope)new DryIocServiceScope(scopedResolver.WithCurrentScope(iscope));
    //       }
    //   }
}
