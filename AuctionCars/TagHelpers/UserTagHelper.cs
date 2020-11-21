using Data;
using Microsoft.AspNetCore.Razor.TagHelpers;


namespace AuctionCars.TagHelpers
{
    public class UserTagHelper: TagHelper
    {
        public object Syl { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            User user = (User)Syl;
            output.Attributes.SetAttribute("href", "/Account/Profile/" + user.Id);
            output.Content.SetContent(user.UserName);
        }
    }
}
