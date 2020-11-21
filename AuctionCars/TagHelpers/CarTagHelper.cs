using Data;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCars.TagHelpers
{
    public class CarTagHelper : TagHelper
    {
        public object Obj { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
                                  
            CarLot lot = (CarLot)Obj;
            output.Attributes.SetAttribute("href", "/Lot/" + lot.Id);
            output.Content.SetContent(lot.Name);
                
            
        }
    }
}
