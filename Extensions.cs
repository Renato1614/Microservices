﻿using Play.Catalog.Service.Models;
using static Play.Catalog.Service.Dtos.Dtos;

namespace Play.Catalog.Service
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name,  item.Description, item.Price, item.CreatedAt);
        }
    }
}
