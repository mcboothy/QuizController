using Microsoft.AspNetCore.Components;
using QuizController.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizController.Shared
{
    public class MainLayoutComponent : LayoutComponentBase
    {
        //[Inject] 
        //public MainViewModel ViewModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
           //await ViewModel.OnInit();
        }
    }
}
