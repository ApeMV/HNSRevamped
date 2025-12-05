// https://github.com/tukasa0001/TownOfHost/blob/main/Modules/OptionItems/BooleanOptionItem.cs
namespace HNSRevamped
{
    public class BooleanOptionItem : OptionItem
    {
        public BooleanOptionItem(int id, string name, bool defaultValue, TabGroup tab, bool isSingleValue)
        : base(id, name, defaultValue ? 1 : 0, tab, isSingleValue)
        {
        }
        public static BooleanOptionItem Create(
            int id, string name, bool defaultValue, TabGroup tab, bool isSingleValue
        )
        {
            return new BooleanOptionItem(
                id, name, defaultValue, tab, isSingleValue
            );
        }

        public override string GetString()
        {
            return GetBool() ? "✓" : "X";
        }

        public override void SetValue(int value)
        {
            base.SetValue(value % 2 == 0 ? 0 : 1);
        }
    }
}