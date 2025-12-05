// https://github.com/tukasa0001/TownOfHost/blob/main/Modules/OptionItems/StringOptionItem.cs
namespace HNSRevamped
{
    public class StringOptionItem : OptionItem
    {
        public IntegerValueRule Rule;
        public string[] Selections;

        public StringOptionItem(int id, string name, int defaultValue, TabGroup tab, bool isSingleValue, string[] selections)
        : base(id, name, defaultValue, tab, isSingleValue)
        {
            Rule = (0, selections.Length - 1, 1);
            Selections = selections;
        }
        public static StringOptionItem Create(
            int id, string name, string[] selections, int defaultIndex, TabGroup tab, bool isSingleValue
        )
        {
            return new StringOptionItem(
                id, name, defaultIndex, tab, isSingleValue, selections
            );
        }

        public override int GetInt() => Rule.GetValueByIndex(CurrentValue);
        public override float GetFloat() => Rule.GetValueByIndex(CurrentValue);
        public override string GetString()
        {
            return Selections[Rule.GetValueByIndex(CurrentValue)];
        }
        public int GetChance()
        {
            if (Selections.Length == 2) return CurrentValue * 100;

            var offset = 12 - Selections.Length;
            var index = CurrentValue + offset;
            var rate = index <= 1 ? index * 5 : (index - 1) * 10;
            return rate;
        }
        public override int GetValue()
            => Rule.RepeatIndex(base.GetValue());

        public override void SetValue(int value)
        {
            base.SetValue(Rule.RepeatIndex(value));
        }
    }
}