using Anthropic = Anthropic;
using Messages = Anthropic.Models.Messages;
using Serialization = System.Text.Json.Serialization;
using ToolUnionProperties = Anthropic.Models.Messages.ToolUnionProperties;

namespace Anthropic.Models.Messages.ToolUnionVariants;

[Serialization::JsonConverter(typeof(Anthropic::VariantConverter<Tool, Messages::Tool>))]
public sealed record class Tool(Messages::Tool Value)
    : Messages::ToolUnion,
        Anthropic::IVariant<Tool, Messages::Tool>
{
    public static Tool From(Messages::Tool value)
    {
        return new(value);
    }

    public override void Validate()
    {
        this.Value.Validate();
    }
}

[Serialization::JsonConverter(
    typeof(Anthropic::VariantConverter<ToolBash20250124, Messages::ToolBash20250124>)
)]
public sealed record class ToolBash20250124(Messages::ToolBash20250124 Value)
    : Messages::ToolUnion,
        Anthropic::IVariant<ToolBash20250124, Messages::ToolBash20250124>
{
    public static ToolBash20250124 From(Messages::ToolBash20250124 value)
    {
        return new(value);
    }

    public override void Validate()
    {
        this.Value.Validate();
    }
}

[Serialization::JsonConverter(
    typeof(Anthropic::VariantConverter<ToolTextEditor20250124, Messages::ToolTextEditor20250124>)
)]
public sealed record class ToolTextEditor20250124(Messages::ToolTextEditor20250124 Value)
    : Messages::ToolUnion,
        Anthropic::IVariant<ToolTextEditor20250124, Messages::ToolTextEditor20250124>
{
    public static ToolTextEditor20250124 From(Messages::ToolTextEditor20250124 value)
    {
        return new(value);
    }

    public override void Validate()
    {
        this.Value.Validate();
    }
}

[Serialization::JsonConverter(
    typeof(Anthropic::VariantConverter<TextEditor20250429, ToolUnionProperties::TextEditor20250429>)
)]
public sealed record class TextEditor20250429(ToolUnionProperties::TextEditor20250429 Value)
    : Messages::ToolUnion,
        Anthropic::IVariant<TextEditor20250429, ToolUnionProperties::TextEditor20250429>
{
    public static TextEditor20250429 From(ToolUnionProperties::TextEditor20250429 value)
    {
        return new(value);
    }

    public override void Validate()
    {
        this.Value.Validate();
    }
}

[Serialization::JsonConverter(
    typeof(Anthropic::VariantConverter<ToolTextEditor20250728, Messages::ToolTextEditor20250728>)
)]
public sealed record class ToolTextEditor20250728(Messages::ToolTextEditor20250728 Value)
    : Messages::ToolUnion,
        Anthropic::IVariant<ToolTextEditor20250728, Messages::ToolTextEditor20250728>
{
    public static ToolTextEditor20250728 From(Messages::ToolTextEditor20250728 value)
    {
        return new(value);
    }

    public override void Validate()
    {
        this.Value.Validate();
    }
}

[Serialization::JsonConverter(
    typeof(Anthropic::VariantConverter<WebSearchTool20250305, Messages::WebSearchTool20250305>)
)]
public sealed record class WebSearchTool20250305(Messages::WebSearchTool20250305 Value)
    : Messages::ToolUnion,
        Anthropic::IVariant<WebSearchTool20250305, Messages::WebSearchTool20250305>
{
    public static WebSearchTool20250305 From(Messages::WebSearchTool20250305 value)
    {
        return new(value);
    }

    public override void Validate()
    {
        this.Value.Validate();
    }
}
