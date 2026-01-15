using PIQI_Engine.Server.Models;
using PIQI_Engine.Server.Services;
using Newtonsoft.Json.Linq;

namespace PIQI_Engine.Server.Engines.SAMs
{

    public class IKE_SAM_LabResultPlausability : SAMBase
    {

        public IKE_SAM_LabResultPlausability(SAM sam, SAMService samService)
            : base(sam, samService) { }

        public override async Task<PIQISAMResponse> EvaluateAsync(PIQISAMRequest request)
        {
            PIQISAMResponse result = new();
            bool passed = false;

            try
            {
                // Set the message model item
                EvaluationItem evaluationItem = (EvaluationItem)request.EvaluationObject;
                MessageModelItem item = evaluationItem?.MessageItem;
                JToken token = JToken.Parse(item.MessageText);

                //Get date of birth string value
                var dobString = token.SelectToken("$.demographics.birthDate")?.ToString();

                //Get touple of test code and lab result value
                JArray? labResults = token.SelectToken("$.labResults") as JArray;
                if (labResults != null)
                {
                    foreach (JToken labResult in labResults)
                    {
                        string? testCode = labResult.SelectToken("test.codings[0].code")?.ToString();
                        string? resultValue = labResult.SelectToken("resultValue.text")?.ToString();
                        if (dobString != null && testCode != null && resultValue != null)
                        {
                            var response = await _SAMService.CheckLabResultPlausibilityAsync(dobString, testCode, resultValue, request.ParmList);
                            if (response == "PLAUSIBLE")
                            {
                                passed = true;
                            }
                            else if (response == "IMPLAUSIBLE")
                            {
                                result.Fail("Plausibility check failed");
                                passed = false;
                                break;
                            }
                            else if (response == "UNKNOWN")
                            {
                                result.Skip("Plausibility unknown");
                                passed = true;
                            }
                        }
                    }
                }

                // // Update result
                result.Done(passed);
            }
            catch (Exception ex)
            {
                result.Error(ex.Message);
            }
            return result;
        }
    }
}
