
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using System;
using Data;

namespace Auction.TagHelpers
{
    public class ObjLinkTagHelper : TagHelper
    {
        public object Obj { get; set;  }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            Type type = Obj.GetType();
            if (type == typeof(CarLot))
            {
                CarLot lot = (CarLot)Obj;
                output.Attributes.SetAttribute("href", "/Lot/" + lot.Id);
                if ((await output.GetChildContentAsync()).GetContent().Length == 0)
                {
                    output.Content.SetContent(lot.Name);
                }
            }
            else if (type == typeof(User))
            { 
                User user = (User)Obj;
                output.Attributes.SetAttribute("href", "/Account/Profile/" + user.Id);
                if ((await output.GetChildContentAsync()).GetContent().Length == 0)
                {
                    output.Content.SetContent(user.UserName);
                }
            }            
        }
    }
}
