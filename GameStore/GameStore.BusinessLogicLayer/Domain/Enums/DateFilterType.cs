namespace GameStore.BusinessLogicLayer.Domain.Enums
{
    public enum DateFilterType
    {
        All,
        LastWeek = 7,
        LastMonth = 31,
        LastYear = 365,
        LastTwoYears = 730,
        LastThreeYears = 1095
    }
}