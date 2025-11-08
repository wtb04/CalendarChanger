namespace CalendarChanger.Helpers
{
    public class SchoolLocation
    {
        #region locations

        private static readonly SchoolLocation _waaier = new()
        {
            ShortNames = ["WA"],
            Geo = "52.238622293842646,6.856343853958932",
            AppleLocation = "CAESqQIIrk0Q4vnFlIKzy454GhIJfUf3FYQeSkAREll30QVtG0AihAEKCU5lZGVybGFuZBICTkwaCk92ZXJpanNzZWwqCEVuc2NoZWRlMghFbnNjaGVkZToHNzUyMiBOQkIORW5zY2hlZGUtTm9vcmRSDURyaWVuZXJsb2xhYW5aATViD0RyaWVuZXJsb2xhYW4gNYoBDkVuc2NoZWRlLU5vb3JkigEFTm9vcmQqBldhYWllcjIPRHJpZW5lcmxvbGFhbiA1MhA3NTIyIE5CIEVuc2NoZWRlMglOZWRlcmxhbmQ4L1ABWkUKJAji+cWUgrPLjngSEgl9R/cVhB5KQBESWXfRBW0bQBiuTZADAaIfHAji+cWUgrPLjngaEAoGV2FhaWVyEAAqAm5sQAA="
        };

        private static readonly SchoolLocation _horst = new()
        {
            ShortNames = ["HT", "HR", "NH", "OH", "ZH", "WH"],
            Geo = "52.23769164691128,6.86065949367557",
            AppleLocation = "CAESowIIrk0QtYLuoLX+lrIMGhIJjNsWSGweSkARoBOnf3txG0AiegoJTmVkZXJsYW5kEgJOTBoKT3Zlcmlqc3NlbCoIRW5zY2hlZGUyCEVuc2NoZWRlOgc3NTIyIExXQg5FbnNjaGVkZS1Ob29yZFIIRGUgSG9yc3RaATJiCkRlIEhvcnN0IDKKAQ5FbnNjaGVkZS1Ob29yZIoBBU5vb3JkKgtIb3JzdCBUb3dlcjIKRGUgSG9yc3QgMjIQNzUyMiBMVyBFbnNjaGVkZTIJTmVkZXJsYW5kOC9QAVpKCiQItYLuoLX+lrIMEhIJjNsWSGweSkARoBOnf3txG0AYrk2QAwGiHyEItYLuoLX+lrIMGhUKC0hvcnN0IFRvd2VyEAAqAm5sQAA="
        };

        private static readonly SchoolLocation _ravelijn = new()
        {
            ShortNames = ["RA"],
            Geo = "52.23945204127462,6.855698284638601",
            AppleLocation = "CAESsAIIrk0Q6dvTxIH+5OKlARoSCXpK3XKcHkpAEZTEryUsbBtAIoQBCglOZWRlcmxhbmQSAk5MGgpPdmVyaWpzc2VsKghFbnNjaGVkZTIIRW5zY2hlZGU6Bzc1MjIgTkJCDkVuc2NoZWRlLU5vb3JkUg1EcmllbmVybG9sYWFuWgE1Yg9EcmllbmVybG9sYWFuIDWKAQ5FbnNjaGVkZS1Ob29yZIoBBU5vb3JkKghSYXZlbGlqbjIPRHJpZW5lcmxvbGFhbiA1MhA3NTIyIE5CIEVuc2NoZWRlMglOZWRlcmxhbmQ4L1ABWkkKJQjp29PEgf7k4qUBEhIJekrdcpweSkARlMSvJSxsG0AYrk2QAwGiHx8I6dvTxIH+5OKlARoSCghSYXZlbGlqbhAAKgJubEAA"
        };

        private static readonly SchoolLocation _technohal = new()
        {
            ShortNames = ["TL"],
            Geo = "52.23766799526685,6.856466640463693",
            AppleLocation = "CAESpQIIrk0QxO+eoqm1sOgJGhIJKuvtgXIeSkAR63UyBZFsG0AifgoJTmVkZXJsYW5kEgJOTBoKT3Zlcmlqc3NlbCoIRW5zY2hlZGUyCEVuc2NoZWRlOgc3NTIyIE5IQg5FbnNjaGVkZS1Ob29yZFIJSGFsbGVud2VnWgIxMGIMSGFsbGVud2VnIDEwigEORW5zY2hlZGUtTm9vcmSKAQVOb29yZCoJVGVjaG5vaGFsMgxIYWxsZW53ZWcgMTAyEDc1MjIgTkggRW5zY2hlZGUyCU5lZGVybGFuZDgvUAFaSAokCMTvnqKptbDoCRISCSrr7YFyHkpAEet1MgWRbBtAGK5NkAMBoh8fCMTvnqKptbDoCRoTCglUZWNobm9oYWwQACoCbmxAAA=="
        };

        private static readonly SchoolLocation _spiegel = new()
        {
            ShortNames = ["SP"],
            Geo = "52.239746870887544,6.849508913473506",
            AppleLocation = "CAESgAIIrk0Qudj0odmbtY9eGhIJB0XSBaweSkARW47tZXlmG0AiYQoJTmVkZXJsYW5kEgJOTBoKT3Zlcmlqc3NlbCoIRW5zY2hlZGUyCEVuc2NoZWRlOgc3NTIyIE5CQg5FbnNjaGVkZS1Ob29yZIoBDkVuc2NoZWRlLU5vb3JkigEFTm9vcmQqDFNwaWVnZWwgKFNQKTIQNzUyMiBOQiBFbnNjaGVkZTIJTmVkZXJsYW5kOC9QAVpLCiQIudj0odmbtY9eEhIJB0XSBaweSkARW47tZXlmG0AYrk2QAwGiHyIIudj0odmbtY9eGhYKDFNwaWVnZWwgKFNQKRAAKgJubEAA"
        };

        private static readonly SchoolLocation _vrijhof = new()
        {
            ShortNames = ["VR"],
            Geo = "52.24310238239606,6.853919526968756",
            AppleLocation = "CAESpQIIrk0Qn6bMk7aZz8onGhIJF2U2yCQfSkARJ9pVSPlpG0AigAEKCU5lZGVybGFuZBICTkwaCk92ZXJpanNzZWwqCEVuc2NoZWRlMghFbnNjaGVkZToHNzUyMiBOTUIORW5zY2hlZGUtTm9vcmRSC0RlIFZlbGRtYWF0WgE1Yg1EZSBWZWxkbWFhdCA1igEORW5zY2hlZGUtTm9vcmSKAQVOb29yZCoHVnJpamhvZjINRGUgVmVsZG1hYXQgNTIQNzUyMiBOTSBFbnNjaGVkZTIJTmVkZXJsYW5kOC9QAVpGCiQIn6bMk7aZz8onEhIJF2U2yCQfSkARJ9pVSPlpG0AYrk2QAwGiHx0In6bMk7aZz8onGhEKB1ZyaWpob2YQACoCbmxAAA=="
        };

        private static readonly SchoolLocation _zilverling = new()
        {
            ShortNames = ["ZI"],
            Geo = "52.2393468114129,6.856991557648326",
            AppleLocation = "CAESpwIIrk0QhpGM57DMwvQcGhIJV56bkZweSkARpgP+lTptG0AifgoJTmVkZXJsYW5kEgJOTBoKT3Zlcmlqc3NlbCoIRW5zY2hlZGUyCEVuc2NoZWRlOgc3NTIyIE5IQg5FbnNjaGVkZS1Ob29yZFIJSGFsbGVud2VnWgIxOWIMSGFsbGVud2VnIDE5igEORW5zY2hlZGUtTm9vcmSKAQVOb29yZCoKWmlsdmVybGluZzIMSGFsbGVud2VnIDE5MhA3NTIyIE5IIEVuc2NoZWRlMglOZWRlcmxhbmQ4L1ABWkkKJAiGkYznsMzC9BwSEglXnpuRnB5KQBGmA/6VOm0bQBiuTZADAaIfIAiGkYznsMzC9BwaFAoKWmlsdmVybGluZxAAKgJubEAA"
        };

        private static readonly SchoolLocation _cubicus = new()
        {
            ShortNames = ["CU"],
            Geo = "52.24061366743407,6.856715182793919",
            AppleLocation = "CAESrgIIrk0QoqiA+q+BhapIGhIJggUX7c4eSkARRJbCnDVtG0AijQEKCU5lZGVybGFuZBICTkwaCk92ZXJpanNzZWwqCEVuc2NoZWRlMghFbnNjaGVkZToHNzUyMiBOSkIORW5zY2hlZGUtTm9vcmRSBkRlIFp1bFoCMTBiCURlIFp1bCAxMHITVW5pdmVyc2l0ZWl0IFR3ZW50ZYoBDkVuc2NoZWRlLU5vb3JkigEFTm9vcmQqB0N1YmljdXMyCURlIFp1bCAxMDIQNzUyMiBOSiBFbnNjaGVkZTIJTmVkZXJsYW5kOC9QAVpGCiQIoqiA+q+BhapIEhIJggUX7c4eSkARRJbCnDVtG0AYrk2QAwGiHx0IoqiA+q+BhapIGhEKB0N1YmljdXMQACoCbmxAAA=="
        };

        private static readonly SchoolLocation _citadel = new()
        {
            ShortNames = ["CI"],
            Geo = "52.23885338233046,6.855084882793879",
            AppleLocation = "CAES+QEIrk0QyMj4zuq1oNSzARoSCQq+HNWXHkpAETDY+DrjaxtAImEKCU5lZGVybGFuZBICTkwaCk92ZXJpanNzZWwqCEVuc2NoZWRlMghFbnNjaGVkZToHNzUyMiBOSEIORW5zY2hlZGUtTm9vcmSKAQ5FbnNjaGVkZS1Ob29yZIoBBU5vb3JkKgdDaXRhZGVsMhA3NTIyIE5IIEVuc2NoZWRlMglOZWRlcmxhbmQ4L1ABWkgKJQjIyPjO6rWg1LMBEhIJCr4c1ZceSkARMNj4OuNrG0AYrk2QAwGiHx4IyMj4zuq1oNSzARoRCgdDaXRhZGVsEAAqAm5sQAA="
        };

        private static readonly SchoolLocation _carre = new()
        {
            ShortNames = ["CR"],
            Geo = "52.23855619328009,6.856412396306683",
            AppleLocation = "CAESqgIIrk0Q5oiy/tf6iP+SARoSCSjS/ZyCHkpAEfTX0HJFbhtAIoABCgtOZXRoZXJsYW5kcxICTkwaCk92ZXJpanNzZWwqCEVuc2NoZWRlMghFbnNjaGVkZToHNzUyMiBOSEIORW5zY2hlZGUtTm9ydGhSCUhhbGxlbndlZ1oCMjNiDEhhbGxlbndlZyAyM4oBDkVuc2NoZWRlLU5vcnRoigEFTm9vcmQqBkNhcnLDqTIMSGFsbGVud2VnIDIzMhA3NTIyIE5IIEVuc2NoZWRlMgtOZXRoZXJsYW5kczgvUAFaSgooCOaIsv7X+oj/kgESEgko0v2cgh5KQBH019ByRW4bQBiuTZADAZgDAaIfHQjmiLL+1/qI/5IBGhAKBkNhcnLDqRAAKgJubEAA",
        };

        private static readonly SchoolLocation _sportcentrum = new()
        {
            ShortNames = ["SC", "Sportcentrum"],
            Geo = "52.24388880438115,6.8503667404817765",
            AppleLocation = "CAESqAIIrk0QiOSr5Jae34KYARoSCRByHcU6H0pAEQQxICAgZxtAInoKCU5lZGVybGFuZBICTkwaCk92ZXJpanNzZWwqCEVuc2NoZWRlMghFbnNjaGVkZToHNzUyMiBOTEIORW5zY2hlZGUtTm9vcmRSB0RlIEhlbXNaAjIwYgpEZSBIZW1zIDIwigEORW5zY2hlZGUtTm9vcmSKAQVOb29yZCoMU3BvcnRjZW50cnVtMgpEZSBIZW1zIDIwMhA3NTIyIE5MIEVuc2NoZWRlMglOZWRlcmxhbmQ4L1ABWk0KJQiI5Kvklp7fgpgBEhIJEHIdxTofSkARBDEgICBnG0AYrk2QAwGiHyMIiOSr5Jae34KYARoWCgxTcG9ydGNlbnRydW0QACoCbmxAAA=="
        };

        private static readonly SchoolLocation _therm = new()
        {
            ShortNames = ["Therm"],
            Geo = "52.23664328306091,6.843018580966755",
            AppleLocation = "CAES4QIIrk0Q8syw+rT3+svgARoSCUGYfVJEHkpAEQAAADBVXxtAIr8BCglOZWRlcmxhbmQSAk5MGgpPdmVyaWpzc2VsKghFbnNjaGVkZTIIRW5zY2hlZGU6Bzc1MjEgUExCH0JlZHJpamZzdGVycmVpbmVuIEVuc2NoZWRlLVdlc3RSCENhcGl0b29sWgI0MGILQ2FwaXRvb2wgNDByH0J1c2luZXNzIGVuIFNjaWVuY2UgUGFyayBUd2VudGWKAR9CZWRyaWpmc3RlcnJlaW5lbiBFbnNjaGVkZS1XZXN0igEFTm9vcmQqBVRoZXJtMgtDYXBpdG9vbCA0MDIQNzUyMSBQTCBFbnNjaGVkZTIJTmVkZXJsYW5kOC9QAVpGCiUI8syw+rT3+svgARISCUGYfVJEHkpAEQAAADBVXxtAGK5NkAMBoh8cCPLMsPq09/rL4AEaDwoFVGhlcm0QACoCbmxAAA=="
        };

        private static readonly SchoolLocation _langezijds = new()
        {
            ShortNames = ["LA"],
            Geo = "52.237913211371804,6.854447153958907",
            AppleLocation = "CAESqwIIrk0Q6LejjaaIweBQGhIJFdkYhW4eSkARd2aC4VxrG0AifgoLTmV0aGVybGFuZHMSAk5MGgpPdmVyaWpzc2VsKghFbnNjaGVkZTIIRW5zY2hlZGU6Bzc1MjIgTkhCDkVuc2NoZWRlLU5vcnRoUglIYWxsZW53ZWdaAThiC0hhbGxlbndlZyA4igEORW5zY2hlZGUtTm9ydGiKAQVOb29yZCoKTGFuZ2V6aWpkczILSGFsbGVud2VnIDgyEDc1MjIgTkggRW5zY2hlZGUyC05ldGhlcmxhbmRzOC9QAVpMCicI6LejjaaIweBQEhIJFdkYhW4eSkARd2aC4VxrG0AYrk2QAwGYAwGiHyAI6LejjaaIweBQGhQKCkxhbmdlemlqZHMQACoCbmxAAA=="
        };
        
        private static readonly SchoolLocation _drienerburght = new()
        {
            ShortNames = ["DR"],
            Geo = "52.24270791987203,6.853873819453504",
            AppleLocation = "CAESvgIIrk0QobCE7Y6N2IrdARoSCV4f50cNH0pAEen4wyo5ahtAIoQBCgtOZXRoZXJsYW5kcxICTkwaCk92ZXJpanNzZWwqCEVuc2NoZWRlMghFbnNjaGVkZToHNzUyMiBMVkIORW5zY2hlZGUtTm9ydGhSDEJvZXJkZXJpandlZ1oBMWIOQm9lcmRlcmlqd2VnIDGKAQ5FbnNjaGVkZS1Ob3J0aIoBBU5vb3JkKg1EcmllbmVyYnVyZ2h0Mg5Cb2VyZGVyaWp3ZWcgMTIQNzUyMiBMViBFbnNjaGVkZTILTmV0aGVybGFuZHM4L1ABWlEKKAihsITtjo3Yit0BEhIJXh/nRw0fSkAR6fjDKjlqG0AYrk2QAwGYAwGiHyQIobCE7Y6N2IrdARoXCg1EcmllbmVyYnVyZ2h0EAAqAm5sQAA="
        };
        
        #endregion

        private static readonly List<SchoolLocation?> Locations =
        [
            _waaier, _horst, _ravelijn, _technohal, _spiegel, _vrijhof, _zilverling, _cubicus, _citadel, _carre,
            _sportcentrum, _therm, _langezijds, _drienerburght
        ];

        public required string[] ShortNames { get; init; }
        public required string AppleLocation { get; init; }
        public required string Geo { get; init;  }

        public static SchoolLocation? GetAppleLocation(string locationString)
        {
            foreach (var location in Locations)
            {
                if (location != null)
                    if ((location?.ShortNames).Any(locationString.StartsWith))
                    {
                        return location;
                    }
            }

            return null;
        }

    }
}
