namespace WebapiTemplate.Dto
{
    public partial class DropdownDto {
        public string Value { get; set; }
        public string Label { get; set; }

        public DropdownDto(string value, string label)
        {
            Value = value;
            Label = label;
        }
    }
}