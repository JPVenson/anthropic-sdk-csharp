using Anthropic = Anthropic;
using CodeAnalysis = System.Diagnostics.CodeAnalysis;
using Generic = System.Collections.Generic;
using Json = System.Text.Json;
using Serialization = System.Text.Json.Serialization;
using System = System;

namespace Anthropic.Models.Messages;

[Serialization::JsonConverter(typeof(Anthropic::ModelConverter<ToolTextEditor20250728>))]
public sealed record class ToolTextEditor20250728
    : Anthropic::ModelBase,
        Anthropic::IFromRaw<ToolTextEditor20250728>
{
    /// <summary>
    /// Name of the tool.
    ///
    /// This is how the tool will be called by the model and in `tool_use` blocks.
    /// </summary>
    public Json::JsonElement Name
    {
        get
        {
            if (!this.Properties.TryGetValue("name", out Json::JsonElement element))
                throw new System::ArgumentOutOfRangeException("name", "Missing required argument");

            return Json::JsonSerializer.Deserialize<Json::JsonElement>(element);
        }
        set { this.Properties["name"] = Json::JsonSerializer.SerializeToElement(value); }
    }

    public Json::JsonElement Type
    {
        get
        {
            if (!this.Properties.TryGetValue("type", out Json::JsonElement element))
                throw new System::ArgumentOutOfRangeException("type", "Missing required argument");

            return Json::JsonSerializer.Deserialize<Json::JsonElement>(element);
        }
        set { this.Properties["type"] = Json::JsonSerializer.SerializeToElement(value); }
    }

    /// <summary>
    /// Create a cache control breakpoint at this content block.
    /// </summary>
    public CacheControlEphemeral? CacheControl
    {
        get
        {
            if (!this.Properties.TryGetValue("cache_control", out Json::JsonElement element))
                return null;

            return Json::JsonSerializer.Deserialize<CacheControlEphemeral?>(element);
        }
        set { this.Properties["cache_control"] = Json::JsonSerializer.SerializeToElement(value); }
    }

    /// <summary>
    /// Maximum number of characters to display when viewing a file. If not specified,
    /// defaults to displaying the full file.
    /// </summary>
    public long? MaxCharacters
    {
        get
        {
            if (!this.Properties.TryGetValue("max_characters", out Json::JsonElement element))
                return null;

            return Json::JsonSerializer.Deserialize<long?>(element);
        }
        set { this.Properties["max_characters"] = Json::JsonSerializer.SerializeToElement(value); }
    }

    public override void Validate()
    {
        this.CacheControl?.Validate();
        _ = this.MaxCharacters;
    }

    public ToolTextEditor20250728()
    {
        this.Name = Json::JsonSerializer.Deserialize<Json::JsonElement>(
            "\"str_replace_based_edit_tool\""
        );
        this.Type = Json::JsonSerializer.Deserialize<Json::JsonElement>("\"text_editor_20250728\"");
    }

#pragma warning disable CS8618
    [CodeAnalysis::SetsRequiredMembers]
    ToolTextEditor20250728(Generic::Dictionary<string, Json::JsonElement> properties)
    {
        Properties = properties;
    }
#pragma warning restore CS8618

    public static ToolTextEditor20250728 FromRawUnchecked(
        Generic::Dictionary<string, Json::JsonElement> properties
    )
    {
        return new(properties);
    }
}
