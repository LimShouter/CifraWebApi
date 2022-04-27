using System.Net;
using System.Net.Mail;
using iText.Html2pdf;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Tls;
using PDF_Report;
using SharedLibrary;

namespace TelegramBot.Services;

public class EmailReportService
{
	

	public void SendDifferentReports(SessionData[] datas, string email)
	{
		var message = new MailMessage();
		message.Subject = "Протокол допуска";
		message.From = new MailAddress("ayva.cifrazone@gmail.com", "AYVA");
		message.To.Add(new MailAddress(email));

		foreach (var sessionData in datas)
		{
			MonitoringReport report;
			if (sessionData.TD3 != null)
			{
				report = new StandartMonitoringReport();
				report.ParamsReplace["DTCTR_1_DATA"] = String.Format("{0:E2}", sessionData.TD1);     
				report.ParamsReplace["DTCTR_2_DATA"] = String.Format("{0:E2}", sessionData.TD2);     
			}
			else
			{
				report = new NonStandartMonitoringReport();                                  
				report.ParamsReplace["DTCTR_1_DATA"] = String.Format("{0:E2}", sessionData.TD1);
				report.ParamsReplace["DTCTR_2_DATA"] = String.Format("{0:E2}", sessionData.TD2);
				report.ParamsReplace["DTCTR_3_DATA"] = String.Format("{0:E2}", sessionData.TD3);
				report.ParamsReplace["DTCTR_4_DATA"] = String.Format("{0:E2}", sessionData.TD4);
				report.ParamsReplace["DTCTR_5_DATA"] = String.Format("{0:E2}", sessionData.TD5);
				report.ParamsReplace["DTCTR_6_DATA"] = String.Format("{0:E2}", sessionData.TD6);
				report.ParamsReplace["DTCTR_7_DATA"] = String.Format("{0:E2}", sessionData.TD7);
				report.ParamsReplace["DTCTR_8_DATA"] = String.Format("{0:E2}", sessionData.TD8);
				report.ParamsReplace["DTCTR_9_DATA"] = String.Format("{0:E2}", sessionData.TD9);
			}

			using (FileStream stream = new FileStream("/pdf/AdmissionReport" + sessionData.AdmissionReportNumber,
				       FileMode.OpenOrCreate))
			{
				HtmlConverter.ConvertToPdf(report.GetHtml(), stream);
				message.Attachments.Add(new Attachment(stream, sessionData.AdmissionReportNumber));
			}
		}

		SendMessage(message);
	}

	public void SendNonStandartReports(List<SessionData> datas, string email)
	{
		var message = new MailMessage();
		message.Subject = "Протокол допуска";
		message.From = new MailAddress("ayva.cifrazone@gmail.com", "AYVA");
		message.To.Add(new MailAddress(email));
		foreach (var sessionData in datas)
		{
			NonStandartMonitoringReport report = new NonStandartMonitoringReport();
			Console.WriteLine(JsonConvert.SerializeObject(sessionData));
			
			report.ParamsReplace["[ABB]"] = String.Format($"AAA");  
			report.ParamsReplace["[YEAR]"] = String.Format($"2022");  
			report.ParamsReplace["[ION_NAME]"] = String.Format($"{sessionData.Agent.EnvironmentId}");  
			report.ParamsReplace["[ION_NUM]"] = String.Format($"{sessionData.Agent.Id}");  
			report.ParamsReplace["[PRTCL_NUM]"] = String.Format($"{sessionData.SessionNumber}");  
			report.ParamsReplace["[SESSION_NUM]"] = String.Format($"{sessionData.SessionNumber}");  
			report.ParamsReplace["[BENCH]"] = String.Format($"ТЧК");  
			report.ParamsReplace["[ORG_NAME]"] = String.Format($"{sessionData.Organization}");  
			report.ParamsReplace["[CIPER]"] = String.Format($"11111");  
			report.ParamsReplace["[IRR_PRODUCT]"] = String.Format($"{sessionData.Objects}");  
			report.ParamsReplace["[START_DATA]"] = String.Format($"{sessionData.Timing.StartTime}");  
			report.ParamsReplace["[START_TIME]"] = String.Format($"{sessionData.Timing.EndTime}");  
			report.ParamsReplace["[DURATION]"] = String.Format($"{sessionData.Timing.IrradiationDuration}");  
			report.ParamsReplace["[EXP_CONDITION]"] = String.Format($"???");
			report.ParamsReplace["[ANGLE]"] = String.Format($"{sessionData.Angle}");  
			report.ParamsReplace["[TEMP]"] = String.Format($"{sessionData.SessionTemperature}");  
			report.ParamsReplace["[DEG_MATERIAL]"] = String.Format($"-");  
			report.ParamsReplace["[THICKNESS]"] = String.Format($"-");  
			report.ParamsReplace["[E_VALUE]"] = String.Format($"{sessionData.Agent.ObjectSurfaceEnergy:E2}");  
			report.ParamsReplace["[E_MEASURE]"] = String.Format($"{sessionData.Agent.ObjectEnergySetupError:E2}");  
			report.ParamsReplace["[MILEAGE_VALUE]"] = String.Format($"{sessionData.Agent.Mileage:E2}");  
			report.ParamsReplace["[MILEAGE_MEASURE]"] = String.Format($"{sessionData.Agent.MileageSetupError:E2}");  
			report.ParamsReplace["[LINE_LOSS_VALUE]"] = String.Format($"{sessionData.Agent.LPP:E2}");  
			report.ParamsReplace["[LINE_LOSS_MEASURE]"] = String.Format($"{sessionData.Agent.LPPSetupError:E2}");  
			report.ParamsReplace["[K_VALUE]"] = String.Format($"{sessionData.K:E2}");  
			report.ParamsReplace["[K_MEASURE]"] = String.Format($"{sessionData.Error:E2}");  
			report.ParamsReplace["[COUNTER_1_DATA]"] = String.Format($"{sessionData.OD1:E2}");  
			report.ParamsReplace["[COUNTER_2_DATA]"] = String.Format($"{sessionData.OD2:E2}");  
			report.ParamsReplace["[COUNTER_3_DATA]"] = String.Format($"{sessionData.OD3:E2}");  
			report.ParamsReplace["[COUNTER_4_DATA]"] = String.Format($"{sessionData.OD4:E2}");  
			report.ParamsReplace["[AVG_DATA_VALUE]"] = String.Format($"{sessionData.ODAverage:E2}");  
			report.ParamsReplace["[ADM_PRTCL_NUM]"] = String.Format($"{sessionData.AdmissionReportNumber}");  
			report.ParamsReplace["[DTCTR_1_DATA]"] = String.Format($"{sessionData.TD1:E2}");  
			report.ParamsReplace["[DTCTR_2_DATA]"] = String.Format($"{sessionData.TD2:E2}");  
			report.ParamsReplace["[DTCTR_3_DATA]"] = String.Format($"{sessionData.TD3:E2}");  
			report.ParamsReplace["[DTCTR_4_DATA]"] = String.Format($"{sessionData.TD4:E2}");  
			report.ParamsReplace["[DTCTR_5_DATA]"] = String.Format($"{sessionData.TD5:E2}");  
			report.ParamsReplace["[DTCTR_6_DATA]"] = String.Format($"{sessionData.TD6:E2}");  
			report.ParamsReplace["[DTCTR_7_DATA]"] = String.Format($"{sessionData.TD7:E2}");  
			report.ParamsReplace["[DTCTR_8_DATA]"] = String.Format($"{sessionData.TD8:E2}");
			report.ParamsReplace["[DTCTR_9_DATA]"] = String.Format($"{sessionData.TD9:E2}");
			
			report.ConvertToPDF(@"Services\PDF\" + sessionData.SessionNumber +".pdf");
			message.Attachments.Add(new Attachment(@"Services\PDF\" + sessionData.SessionNumber + ".pdf"));
		}

		SendMessage(message);
	}

	public void SendStandartReports(SessionData[] datas, string email)
	{
		var message = new MailMessage();
		message.Subject = "Протокол допуска";
		message.From = new MailAddress("ayva.cifrazone@gmail.com", "AYVA");
		message.To.Add(new MailAddress(email));
		foreach (var sessionData in datas)
		{
			StandartMonitoringReport report = new StandartMonitoringReport();
			report.ParamsReplace["DTCTR_1_DATA"] = String.Format("{0:E2}", sessionData.TD1);
			report.ParamsReplace["DTCTR_2_DATA"] = String.Format("{0:E2}", sessionData.TD2);
			
			using (FileStream stream = new FileStream("/pdf/AdmissionReport" + sessionData.AdmissionReportNumber,
				       FileMode.OpenOrCreate))
			{
				HtmlConverter.ConvertToPdf(report.GetHtml(), stream);
				message.Attachments.Add(new Attachment(stream, sessionData.AdmissionReportNumber));
			}
		}

		SendMessage(message);
	}

	public void SendAdmissionReports(SessionData[] datas, string email)
	{
		var message = new MailMessage();
		message.Subject = "Протокол допуска";
		message.From = new MailAddress("ayva.cifrazone@gmail.com", "AYVA");
		message.To.Add(new MailAddress(email));
		var protocols = new List<string>();
		foreach (var sessionData in datas)
		{
			if (protocols.Contains(sessionData.AdmissionReportNumber))
			{
				protocols.Add(sessionData.AdmissionReportNumber);
				using (FileStream stream = new FileStream("/pdf/AdmissionReport" + sessionData.AdmissionReportNumber,
					       FileMode.OpenOrCreate))
				{
					var AdmissionReport = new AdmissionReport();
					AdmissionReport.ParamsReplace["TD_1"] = sessionData.TD1.ToString();
					HtmlConverter.ConvertToPdf(AdmissionReport.GetHtml(), stream);
					message.Attachments.Add(new Attachment(stream, sessionData.AdmissionReportNumber));
				}
			}
		}

		SendMessage(message);
	}

	private void SendMessage(MailMessage message)
	{
		using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
		{
			client.Credentials = new NetworkCredential("ayva.cifrazone@gmail.com", "1q2q3q4w");
			client.EnableSsl = true;
			client.Send(message);
		}
	}
}