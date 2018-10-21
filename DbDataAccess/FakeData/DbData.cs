using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbDataAccess
{
    public static class DbData
    {
        private static Guid _bulbId1 = Guid.NewGuid();
        private static Guid _bulbId2 = Guid.NewGuid();
        private static Guid _bulbId3 = Guid.NewGuid();
        private static Guid _bulbId4 = Guid.NewGuid();
        private static Guid _bulbId5 = Guid.NewGuid();
        private static Guid _bulbId6 = Guid.NewGuid();
        private static Guid _bulbId7 = Guid.NewGuid();
        private static Guid _bulbId8 = Guid.NewGuid();
        private static Guid _bulbId9 = Guid.NewGuid();
        private static Guid _bulbId10 = Guid.NewGuid();
        private static Guid _streetlightId1 = Guid.NewGuid();
        private static Guid _streetlightId2 = Guid.NewGuid();
        private static Guid _streetlightId3 = Guid.NewGuid();

        private static BulbState _bulbState1 = new BulbState()
        {
            BulbCurrentState = new BulbData()
            {
                BulbHours = 500,
                BulbTemperature = 50,
                IsOn = false
            },
            BulbInformation = new BulbModel()
            {
                Id = _bulbId1,
                MaxHours = 4000,
                MaxTemperature = 200,
                PowerDraw = 200
            }
        };

        private static BulbState _bulbState2 = new BulbState()
        {
            BulbCurrentState = new BulbData()
            {
                BulbHours = 2500,
                BulbTemperature = 253,
                IsOn = false,
                FaultCondition = FaultCode.OverTemperature
            },
            BulbInformation = new BulbModel()
            {
                Id = _bulbId2,
                MaxHours = 4000,
                MaxTemperature = 200,
                PowerDraw = 200
            }
        };

        private static BulbState _bulbState3 = new BulbState()
        {
            BulbCurrentState = new BulbData()
            {
                BulbHours = 3500,
                BulbTemperature = 110,
                IsOn = false
            },
            BulbInformation = new BulbModel()
            {
                Id = _bulbId3,
                MaxHours = 4000,
                MaxTemperature = 220,
                PowerDraw = 400
            }
        };

        private static BulbState _bulbState4 = new BulbState()
        {
            BulbCurrentState = new BulbData()
            {
                BulbHours = 2500,
                BulbTemperature = 50,
                IsOn = false
            },
            BulbInformation = new BulbModel()
            {
                Id = _bulbId4,
                MaxHours = 2000,
                MaxTemperature = 200,
                PowerDraw = 200
            }
        };

        private static BulbState _bulbState5 = new BulbState()
        {
            BulbCurrentState = new BulbData()
            {
                BulbHours = 2500,
                BulbTemperature = 50,
                IsOn = false
            },
            BulbInformation = new BulbModel()
            {
                Id = _bulbId5,
                MaxHours = 2000,
                MaxTemperature = 200,
                PowerDraw = 200
            }
        };
        private static BulbState _bulbState6 = new BulbState()
        {
            BulbCurrentState = new BulbData()
            {
                BulbHours = 2500,
                BulbTemperature = 50,
                IsOn = false
            },
            BulbInformation = new BulbModel()
            {
                Id = _bulbId6,
                MaxHours = 2000,
                MaxTemperature = 200,
                PowerDraw = 200
            }
        };
        private static BulbState _bulbState7 = new BulbState()
        {
            BulbCurrentState = new BulbData()
            {
                BulbHours = 2500,
                BulbTemperature = 50,
                IsOn = false
            },
            BulbInformation = new BulbModel()
            {
                Id = _bulbId7,
                MaxHours = 2000,
                MaxTemperature = 200,
                PowerDraw = 200
            }
        };
        private static BulbState _bulbState8 = new BulbState()
        {
            BulbCurrentState = new BulbData()
            {
                BulbHours = 2500,
                BulbTemperature = 50,
                IsOn = false
            },
            BulbInformation = new BulbModel()
            {
                Id = _bulbId8,
                MaxHours = 2000,
                MaxTemperature = 200,
                PowerDraw = 200
            }
        };
        private static BulbState _bulbState9 = new BulbState()
        {
            BulbCurrentState = new BulbData()
            {
                BulbHours = 2500,
                BulbTemperature = 50,
                IsOn = false
            },
            BulbInformation = new BulbModel()
            {
                Id = _bulbId9,
                MaxHours = 2000,
                MaxTemperature = 200,
                PowerDraw = 200
            }
        };
        private static BulbState _bulbState10 = new BulbState()
        {
            BulbCurrentState = new BulbData()
            {
                BulbHours = 2500,
                BulbTemperature = 50,
                IsOn = false
            },
            BulbInformation = new BulbModel()
            {
                Id = _bulbId10,
                MaxHours = 2000,
                MaxTemperature = 200,
                PowerDraw = 200
            }
        };

        public static IEnumerable<BulbState> Bulbs
        {
            get;
        } = new List<BulbState> { _bulbState1, _bulbState2, _bulbState3, _bulbState4, _bulbState5, _bulbState6, _bulbState7, _bulbState8, _bulbState9, _bulbState10 };

        private static StreetlightModel _streetlight1 = new StreetlightModel()
        {
            Bulbs = new List<BulbState> { _bulbState1, _bulbState2, _bulbState3, _bulbState4 },
            Description = "Western Ridge Streetlight",
            Id = _streetlightId1,
            ElectricalDraw = 25,
            IsSwitchedOn = false
        };

        private static StreetlightModel _streetlight2 = new StreetlightModel()
        {
            Bulbs = new List<BulbState> { _bulbState4, _bulbState5, _bulbState6, _bulbState7 },
            Description = "Northern Ridge Streetlight",
            Id = _streetlightId2,
            ElectricalDraw = 35,
            IsSwitchedOn = false
        };

        private static StreetlightModel _streetlight3 = new StreetlightModel()
        {
            Bulbs = new List<BulbState> { _bulbState8, _bulbState9, _bulbState10 },
            Description = "Eastern Ridge Streetlight",
            Id = _streetlightId3,
            ElectricalDraw = 45,
            IsSwitchedOn = false
        };

        public static IEnumerable<StreetlightModel> Streetlights
        {
            get;
        } = new List<StreetlightModel> { _streetlight1, _streetlight2, _streetlight3 };
        public static IEnumerable<StreetlightSummaryModel> StreetlightSummary
        {
            get
            {
                return Streetlights.Select(x => new StreetlightSummaryModel()
                {
                    Id = x.Id,
                    Description = x.Description
                }).ToList();
            }
        }
    }
}
