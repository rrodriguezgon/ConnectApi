using System;
using System.Collections.Generic;
using System.Text;

namespace OrderMailboxHub.Application.Dtos.Orders
{
    public class CreateUsersShopsCommand
    {
        public string UserId { get; private set; }

        public IEnumerable<UserShopModel> UsersShops { get; private set; }

        public CreateUsersShopsCommand(string userId, IEnumerable<UserShopModel> usersShops)
        {
            UserId = userId;
            UsersShops = usersShops;
        }
    }

    public class UserShopModel
    {
        public string CountryId { get; set; }

        public string BussinessAreaId { get; set; }

        public string ShopId { get; set; }
    }
}
