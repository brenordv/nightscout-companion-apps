﻿using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticHbA1CValue : StatisticSimpleFloatValue
{
    [JsonProperty("status")] 
    public HbA1CCalculationStatus Status { get; init; } = HbA1CCalculationStatus.NotCalculated;
}