using Aki.Launcher.Models;
using Aki.Launcher.ViewModels;
using ReactiveUI;
using System;

namespace Aki.Launcher.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class NavigationPreCondition : Attribute
    {
        public abstract NavigationPreConditionResult TestPreCondition(IScreen Host);
    }
}
