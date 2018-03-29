
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;

namespace Azure_doc
{
    public static class Function1
    {
		// memored states
		public static uint bell_state = 0;
		public static uint kurant_state = 0;
		public static uint seq_state = 0;
		public static uint midi_state = 0;

		public static uint zad_bell = 0;
		public static uint zad_kur = 0;
		public static uint zad_seq = 0;
		public static uint zad_midi = 0;


		[FunctionName("Function1")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
			// received params
			string command = null;
			uint bells;
			uint kurant;
			uint seq;
			uint midi;

			string response;

			// log.Info("C# HTTP trigger function processed a request.");

            string frame = req.Query["params"];


			// detekcja parametrow
			string[] params_tab = frame.Split(',');
			int params_cnt = params_tab.Length;

			// konwersja parametrow
			if (params_cnt >= 5)
			{
				command = params_tab[0];

				try
				{
					bells = Convert.ToUInt32(params_tab[1]);
					kurant = Convert.ToUInt32(params_tab[2]);
					seq = Convert.ToUInt32(params_tab[3]);
					midi = Convert.ToUInt32(params_tab[4]);
				}
				catch
				{
					return new BadRequestObjectResult("Err: params problem");
				}

				// response = "4 params Ok";
			}
			else
				return new BadRequestObjectResult("Err: params number");
			

			switch (command)
			{
			case "get":
				bell_state = bells;
				kurant_state = kurant;
				seq_state = seq;
				midi_state = midi;

				response = "set," + zad_bell.ToString() + "," + zad_kur.ToString() + "," + zad_seq.ToString() + "," + zad_midi.ToString();

				zad_bell = 0;
				zad_kur = 0;
				zad_seq = 0;
				zad_midi = 0;
				break;

			case "set":
				zad_bell = bells;
				zad_kur = kurant;
				zad_seq = seq;
				zad_midi = midi;

				response = "ok," + bell_state.ToString() + "," + kurant_state.ToString() + "," + seq_state.ToString() + "," + midi_state.ToString();
				break;

			default:
				response = "Err: Invalid command";
				break;
			}



			return (ActionResult)new OkObjectResult(response);

		}

    }

}



/*
string requestBody = new StreamReader(req.Body).ReadToEnd();
dynamic data = JsonConvert.DeserializeObject(requestBody);
frame = frame ?? data?.name;
*/


// return frame != null ? (ActionResult)new OkObjectResult(response) : new BadRequestObjectResult("Error");
// return name != null ? (ActionResult)new OkObjectResult($"Hello_123_456_, {name}") : new BadRequestObjectResult("Error");
// : new BadRequestObjectResult("Please pass a name on the query string or in the request body");