using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StudyApp.CustomControls
{
    public class FloatingActionButton : View
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create<FloatingActionButton, ICommand>(p => p.Command, null);
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create<FloatingActionButton, object>(p => p.CommandParameter, null);
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly BindableProperty IconProperty = BindableProperty.Create(
            propertyName: "Icon",
            returnType: typeof(string),
            declaringType: typeof(FloatingActionButton),
            defaultValue: "icon_plus");

        public ICommand TransitionCommand
        {
            get
            {
                return new Command(async () =>
                {
                    this.AnchorX = 0.48;
                    this.AnchorY = 0.48;
                    await this.ScaleTo(0.9, 50, Easing.Linear);
                    await Task.Delay(30);
                    await this.ScaleTo(1, 50, Easing.Linear);
                    if (Command != null)
                    {
                        Command.Execute(CommandParameter);
                    }
                });
            }
        }


        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
    }
}
