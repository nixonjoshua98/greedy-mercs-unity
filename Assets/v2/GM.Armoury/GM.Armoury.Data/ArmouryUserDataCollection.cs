using GM.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace GM.Armoury.Data
{
    public class ArmouryUserDataCollection
    {
        List<Models.UserArmouryItemModel> Items;

        public ArmouryUserDataCollection(List<Models.UserArmouryItemModel> items)
        {
            Items = items;
        }

        public Models.UserArmouryItemModel GetItem(int key) => Items.Where(ele => ele.Id == key).FirstOrDefault();

        public List<Models.UserArmouryItemModel> OwnedItems => Items.Where(ele => ele.NumOwned > 0).OrderBy(ele => ele.Id).ToList();

        public void Update(Models.UserArmouryItemModel item)
        {          
            Items.UpdateOrInsertElement(item, (ele) => ele.Id == item.Id);
        }
    }
}
