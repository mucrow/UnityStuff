using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Sandbox2D.WebRequestTest {
  public class WebRequestTest: MonoBehaviour {
    [SerializeField] Text _textElement;
    // https://api.sampleapis.com/switch/games

    void Start() {
      _textElement.text = "Loading...";
      StartCoroutine(GetRequest("https://api.sampleapis.com/switch/games"));
    }

    void ExampleStart() {
      // A correct website page.
      StartCoroutine(GetRequest("https://www.example.com"));

      // A non-existing page.
      StartCoroutine(GetRequest("https://error.html"));
    }

    IEnumerator GetRequest(string uri) {
      using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        string[] pages = uri.Split('/');
        int page = pages.Length - 1;

        switch (webRequest.result) {
          case UnityWebRequest.Result.ConnectionError:
          case UnityWebRequest.Result.DataProcessingError:
            Debug.LogError(pages[page] + ": Error: " + webRequest.error);
            break;
          case UnityWebRequest.Result.ProtocolError:
            Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
            break;
          case UnityWebRequest.Result.Success:
            _textElement.text = webRequest.downloadHandler.text;
            break;
        }
      }
    }
  }
}
