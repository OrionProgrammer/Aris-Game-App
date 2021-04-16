using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;

namespace Aris.ServerTest.ViewModels
{
    public class GamesListViewModel
    {

        public IEnumerable<Models.KoreGame> Games { get; set; }
        
        //Task 5.  Added a list of categories variable
        public IEnumerable<string> Categories { get; set; }

    }
}
