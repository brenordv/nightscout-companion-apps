namespace Raccoon.Ninja.Domain.Core.Enums;

public enum HbA1CCalculationStatus
{
    NotCalculated = 0,
    Complete = 1,
    Partial = 2,

    [Obsolete("For retro compatibility only. Use StatisticDataPoint instead and its properties instead.")]
    Error = 3
}