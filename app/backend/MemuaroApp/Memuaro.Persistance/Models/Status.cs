﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Memuaro.Persistance.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum Status
{
    Unanswered,
    PartlyAnswered,
    Answered
}