using Data;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace AuctionCars.TagHelpers
{
    public class ObjLinkTagHelper : TagHelper
    {
        public object Obj { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            Type type = Obj.GetType();
            if (type == typeof(CarLot))
            {
                CarLot lot = (CarLot)Obj;
                output.Attributes.SetAttribute("href", "/Lot/" + lot.Id);
                output.Content.SetContent(lot.Name);

            }
            else if (type == typeof(User))
            {
                User user = (User)Obj;
                output.Attributes.SetAttribute("href", "/Account/Profile/" + user.Id);
                output.Content.SetContent(user.UserName);

            }
        }
    }
}
