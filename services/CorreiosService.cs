using System.Xml;
using System.Net;
using System.IO;
using System;

namespace UnBank.service {

    public class Correios{
                public static string CallWebService(string Cep)
                {
                    var _url = "http://ws.correios.com.br/calculador/CalcPrecoPrazo.asmx";
                    var _action = "http://tempuri.org/CalcPrecoPrazo";

                    XmlDocument soapEnvelopeXml = CreateSoapEnvelope(Cep);
                    HttpWebRequest webRequest = CreateWebRequest(_url, _action);
                    InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                    // begin async call to web request.
                    IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

                    // suspend this thread until call is complete. You might want to
                    // do something usefull here like update your UI.
                    asyncResult.AsyncWaitHandle.WaitOne();

                    // get the response from the completed web request.
                    string soapResult;
                    using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                    {
                        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                        {
                            soapResult = rd.ReadToEnd();
                        }
                        return soapResult;        
                    }
                }

                private static HttpWebRequest CreateWebRequest(string url, string action)
                {
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                    webRequest.Headers.Add("SOAPAction", action);
                    webRequest.ContentType = "text/xml;";
                    webRequest.Accept = "text/xml";
                    webRequest.Method = "POST";
                    return webRequest;
                }

                private static XmlDocument CreateSoapEnvelope(string Cep)
                {
                    XmlDocument soapEnvelopeDocument = new XmlDocument();
                    soapEnvelopeDocument.LoadXml(
                        "<soap12:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap12='http://www.w3.org/2003/05/soap-envelope'>" +
                                "<soap12:Body>" +
                                    "<CalcPrecoPrazo xmlns='http://tempuri.org/'>" +
                                    "<nCdEmpresa></nCdEmpresa>" +
                                    "<sDsSenha></sDsSenha>" +
                                    "<nCdServico>04014</nCdServico>" +
                                    "<sCepOrigem>40393000</sCepOrigem>" +
                                    $"<sCepDestino>{Cep}</sCepDestino>" +
                                    "<nVlPeso>1</nVlPeso>" +
                                    "<nCdFormato>3</nCdFormato>" +
                                    "<nVlComprimento>5</nVlComprimento>" +
                                    "<nVlAltura>0</nVlAltura>" +
                                    "<nVlLargura>7</nVlLargura>" +
                                    "<nVlDiametro>3</nVlDiametro>" +
                                    "<sCdMaoPropria>S</sCdMaoPropria>" +
                                    "<nVlValorDeclarado>0</nVlValorDeclarado>" +
                                    "<sCdAvisoRecebimento>S</sCdAvisoRecebimento>" +
                                    "</CalcPrecoPrazo>" +
                                "</soap12:Body>" +
                                "</soap12:Envelope>");
                    return soapEnvelopeDocument;
                }

                private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
                {
                    using (Stream stream = webRequest.GetRequestStream())
                    {
                        soapEnvelopeXml.Save(stream);
                    }
                }
    }
}