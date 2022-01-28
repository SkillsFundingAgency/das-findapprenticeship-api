namespace SFA.DAS.FAA.Data.UnitTests.Repository
{
    public static class FakeElasticResponses
    {
        public const string SingleHitResponse =
                @"{""took"":33,""timed_out"":false,""_shards"":{""total"":1,""successful"":1,""skipped"":0,""failed"":0},
                    ""hits"":{""total"":{""value"":1,""relation"":""eq""},""max_score"":null,""hits"":[{""_index"":
                    ""test-faa-apprenticeships.2021-10-08-14-30"",""_type"":""_doc"",""_id"":
                    ""1000006648"",""_score"":1.0,""_source"":{          
                        ""id"" : 1000006648,
                        ""title"" : ""dbcMgHEgpl_14Jul2020_10014932357 apprenticeship"",
                        ""startDate"" : ""2020-10-18T00:00:00Z"",
                        ""closingDate"" : ""2020-09-17T00:00:00Z"",
                        ""postedDate"" : ""2020-07-14T10:06:25.8640000Z"",
                        ""employerName"" : ""ESFA LTD"",
                        ""providerName"" : ""BALTIC TRAINING SERVICES LIMITED"",
                        ""description"" : ""VyNpryuzIdktcJVjqJgxXpSFuwdrkqJRYCqEriOCbfZefEcOMO"",
                        ""numberOfPositions"" : 2,
                        ""isPositiveAboutDisability"" : false,
                        ""isEmployerAnonymous"" : false,
                        ""vacancyLocationType"" : ""NonNational"",
                        ""location"" : {
                            ""lon"" : -3.169768,
                            ""lat"" : 55.970099
                        },
                        ""apprenticeshipLevel"" : ""Intermediate"",
                        ""vacancyReference"" : ""1000006648"",
                        ""category"" : ""Retail and Commercial Enterprise"",
                        ""categoryCode"" : ""SSAT1.HBY"",
                        ""subCategory"" : ""Butchery > Abattoir worker"",
                        ""subCategoryCode"" : ""STDSEC.5"",
                        ""workingWeek"" : ""hIxfvstIfxOZrDC"",
                        ""wageType"" : 3,
                        ""wageText"" : ""£8,049.60 to £14,976.00"",
                        ""wageUnit"" : 4,
                        ""hoursPerWeek"" : 40.0,
                        ""standardLarsCode"" : 274,
                        ""isDisabilityConfident"" : false,
                        ""ukprn"" : 10019026
                }}]}}";
        
        public const string NoHitsResponse =
                @"{""took"":0,""timed_out"":false,
                    ""_shards"":{""total"":1,""successful"":1,""skipped"":0,""failed"":0},
                    ""hits"":{""total"":{""value"":0,""relation"":""eq""},""max_score"":null,""hits"":[]}
                }";
        
        public const string MoreThanOneHitResponse =
                @"{""took"":33,""timed_out"":false,""_shards"":{""total"":1,""successful"":1,""skipped"":0,""failed"":0},
                    ""hits"":{""total"":{""value"":2,""relation"":""eq""},""max_score"":null,""hits"":[{""_index"":
                    ""test-faa-apprenticeships.2021-10-08-14-30"",""_type"":""_doc"",""_id"":
                    ""1000006648"",""_score"":1.0,""_source"":{          
                        ""id"" : 1000006648,
                        ""title"" : ""dbcMgHEgpl_14Jul2020_10014932357 apprenticeship"",
                        ""startDate"" : ""2020-10-18T00:00:00Z"",
                        ""closingDate"" : ""2020-09-17T00:00:00Z"",
                        ""postedDate"" : ""2020-07-14T10:06:25.8640000Z"",
                        ""employerName"" : ""ESFA LTD"",
                        ""providerName"" : ""BALTIC TRAINING SERVICES LIMITED"",
                        ""description"" : ""VyNpryuzIdktcJVjqJgxXpSFuwdrkqJRYCqEriOCbfZefEcOMO"",
                        ""numberOfPositions"" : 2,
                        ""isPositiveAboutDisability"" : false,
                        ""isEmployerAnonymous"" : false,
                        ""vacancyLocationType"" : ""NonNational"",
                        ""location"" : {
                            ""lon"" : -3.169768,
                            ""lat"" : 55.970099
                        },
                        ""apprenticeshipLevel"" : ""Intermediate"",
                        ""vacancyReference"" : ""1000006648"",
                        ""category"" : ""Retail and Commercial Enterprise"",
                        ""categoryCode"" : ""SSAT1.HBY"",
                        ""subCategory"" : ""Butchery > Abattoir worker"",
                        ""subCategoryCode"" : ""STDSEC.5"",
                        ""workingWeek"" : ""hIxfvstIfxOZrDC"",
                        ""wageType"" : 3,
                        ""wageText"" : ""£8,049.60 to £14,976.00"",
                        ""wageUnit"" : 4,
                        ""hoursPerWeek"" : 40.0,
                        ""standardLarsCode"" : 274,
                        ""isDisabilityConfident"" : false,
                        ""ukprn"" : 10019026
                    }},{""_index"":
                    ""test-faa-apprenticeships.2021-10-08-14-30"",""_type"":""_doc"",""_id"":
                    ""1000006648"",""_score"":1.0,""_source"":{          
                        ""id"" : 1000006648,
                        ""title"" : ""dbcMgHEgpl_14Jul2020_10014932357 apprenticeship"",
                        ""startDate"" : ""2020-10-18T00:00:00Z"",
                        ""closingDate"" : ""2020-09-17T00:00:00Z"",
                        ""postedDate"" : ""2020-07-14T10:06:25.8640000Z"",
                        ""employerName"" : ""ESFA LTD"",
                        ""providerName"" : ""BALTIC TRAINING SERVICES LIMITED"",
                        ""description"" : ""VyNpryuzIdktcJVjqJgxXpSFuwdrkqJRYCqEriOCbfZefEcOMO"",
                        ""numberOfPositions"" : 2,
                        ""isPositiveAboutDisability"" : false,
                        ""isEmployerAnonymous"" : false,
                        ""vacancyLocationType"" : ""NonNational"",
                        ""location"" : {
                            ""lon"" : -3.169768,
                            ""lat"" : 55.970099
                        },
                        ""apprenticeshipLevel"" : ""Intermediate"",
                        ""vacancyReference"" : ""1000006648"",
                        ""category"" : ""Retail and Commercial Enterprise"",
                        ""categoryCode"" : ""SSAT1.HBY"",
                        ""subCategory"" : ""Butchery > Abattoir worker"",
                        ""subCategoryCode"" : ""STDSEC.5"",
                        ""workingWeek"" : ""hIxfvstIfxOZrDC"",
                        ""wageType"" : 3,
                        ""wageText"" : ""£8,049.60 to £14,976.00"",
                        ""wageUnit"" : 4,
                        ""hoursPerWeek"" : 40.0,
                        ""standardLarsCode"" : 274,
                        ""isDisabilityConfident"" : false,
                        ""ukprn"" : 10019026
                }}]}}";
        
        public const string MoreThanOneHitResponseWithSort =
                @"{""took"":33,""timed_out"":false,""_shards"":{""total"":1,""successful"":1,""skipped"":0,""failed"":0},
                    ""hits"":{""total"":{""value"":2,""relation"":""eq""},""max_score"":null,""hits"":[{""_index"":
                    ""test-faa-apprenticeships.2021-10-08-14-30"",""_type"":""_doc"",""_id"":
                    ""1000006648"", ""_score"":1.0,""_source"":{          
                        ""id"" : 1000006648,
                        ""title"" : ""dbcMgHEgpl_14Jul2020_10014932357 apprenticeship"",
                        ""startDate"" : ""2020-10-18T00:00:00Z"",
                        ""closingDate"" : ""2020-09-17T00:00:00Z"",
                        ""postedDate"" : ""2020-07-14T10:06:25.8640000Z"",
                        ""employerName"" : ""ESFA LTD"",
                        ""providerName"" : ""BALTIC TRAINING SERVICES LIMITED"",
                        ""description"" : ""VyNpryuzIdktcJVjqJgxXpSFuwdrkqJRYCqEriOCbfZefEcOMO"",
                        ""numberOfPositions"" : 2,
                        ""isPositiveAboutDisability"" : false,
                        ""isEmployerAnonymous"" : false,
                        ""vacancyLocationType"" : ""NonNational"",
                        ""location"" : {
                            ""lon"" : -3.169768,
                            ""lat"" : 55.970099
                        },
                        ""apprenticeshipLevel"" : ""Intermediate"",
                        ""vacancyReference"" : ""1000006648"",
                        ""category"" : ""Retail and Commercial Enterprise"",
                        ""categoryCode"" : ""SSAT1.HBY"",
                        ""subCategory"" : ""Butchery > Abattoir worker"",
                        ""subCategoryCode"" : ""STDSEC.5"",
                        ""workingWeek"" : ""hIxfvstIfxOZrDC"",
                        ""wageType"" : 3,
                        ""wageText"" : ""£8,049.60 to £14,976.00"",
                        ""wageUnit"" : 4,
                        ""hoursPerWeek"" : 40.0,
                        ""standardLarsCode"" : 274,
                        ""isDisabilityConfident"" : false,
                        ""ukprn"" : 10019026
                    }, ""sort"" : [4.5]},{""_index"":
                    ""test-faa-apprenticeships.2021-10-08-14-30"",""_type"":""_doc"",""_id"":
                    ""1000006648"",""_score"":1.0,""_source"":{          
                        ""id"" : 1000006648,
                        ""title"" : ""dbcMgHEgpl_14Jul2020_10014932357 apprenticeship"",
                        ""startDate"" : ""2020-10-18T00:00:00Z"",
                        ""closingDate"" : ""2020-09-17T00:00:00Z"",
                        ""postedDate"" : ""2020-07-14T10:06:25.8640000Z"",
                        ""employerName"" : ""ESFA LTD"",
                        ""providerName"" : ""BALTIC TRAINING SERVICES LIMITED"",
                        ""description"" : ""VyNpryuzIdktcJVjqJgxXpSFuwdrkqJRYCqEriOCbfZefEcOMO"",
                        ""numberOfPositions"" : 2,
                        ""isPositiveAboutDisability"" : false,
                        ""isEmployerAnonymous"" : false,
                        ""vacancyLocationType"" : ""NonNational"",
                        ""location"" : {
                            ""lon"" : -3.169768,
                            ""lat"" : 55.970099
                        },
                        ""apprenticeshipLevel"" : ""Intermediate"",
                        ""vacancyReference"" : ""1000006648"",
                        ""category"" : ""Retail and Commercial Enterprise"",
                        ""categoryCode"" : ""SSAT1.HBY"",
                        ""subCategory"" : ""Butchery > Abattoir worker"",
                        ""subCategoryCode"" : ""STDSEC.5"",
                        ""workingWeek"" : ""hIxfvstIfxOZrDC"",
                        ""wageType"" : 3,
                        ""wageText"" : ""£8,049.60 to £14,976.00"",
                        ""wageUnit"" : 4,
                        ""hoursPerWeek"" : 40.0,
                        ""standardLarsCode"" : 274,
                        ""isDisabilityConfident"" : false,
                        ""ukprn"" : 10019026
                }, ""sort"" : [2.5]}]}}";
    }
}