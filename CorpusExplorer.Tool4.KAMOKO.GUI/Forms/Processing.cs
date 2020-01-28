#region

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CorpusExplorer.Tool4.KAMOKO.GUI.Forms.Abstract;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.GUI.Forms
{
  /// <summary>
  ///   Class <see cref="Processing" />
  /// </summary>
  public partial class Processing : AbstractForm
  {
    /// <summary>
    ///   The _splash form.
    /// </summary>
    private static Processing _splashForm;

    /// <summary>
    ///   The _splash thread.
    /// </summary>
    private static Thread _splashThread;

    /// <summary>
    ///   The locker.
    /// </summary>
    public static readonly object locker = new object();

    /// <summary>
    ///   The wait please.
    /// </summary>
    public static bool WaitPlease = true;

    /// <summary>
    ///   Prevents a default instance of the <see cref="Processing" /> class from being created.
    /// </summary>
    private Processing()
    {
      InitializeComponent();
      Load += (sender, args) => radWaitingBar1.StartWaiting();
    }

    /// <summary>
    ///   Gets or sets the message.
    /// </summary>
    private string Message
    {
      get => radLabel2.Text;

      set
      {
        if (string.IsNullOrEmpty(value))
          value = "Lehnen Sie sich zurück und enspannen Sie sich, während CorpusExplorer für Sie rechnet.";

        radLabel2.Text = value;
      }
    }

    public static void Invoke(string message, Action action)
    {
      SplashShow();
      SplashMessage(message);

      action.Invoke();

      SplashClose(null);
    }

    /// <summary>
    ///   The splash close.
    /// </summary>
    /// <param name="message">
    ///   The message.
    /// </param>
    /// <param name="itWasrinvoked">
    ///   The it wasrinvoked.
    /// </param>
    public static void SplashClose(string message, bool itWasrinvoked = false)
    {
      var launched = false;
      while (!launched &&
             !itWasrinvoked)
        lock (locker)
        {
          if (!WaitPlease)
            launched = true;
        }

      if (_splashForm   == null ||
          _splashThread == null)
        return;

      if (_splashForm.InvokeRequired)
      {
        SplashHandler handle = SplashClose;
        _splashForm.Invoke(handle, message, true);
        return;
      }

      if (!string.IsNullOrEmpty(message))
        _splashForm.Message = message;

      _splashForm.Close();
      _splashForm = null;
      _splashThread = null;
    }

    /// <summary>
    ///   The splash message.
    /// </summary>
    /// <param name="message">
    ///   The message.
    /// </param>
    /// <param name="itWasrinvoked">
    ///   The it wasrinvoked.
    /// </param>
    public static void SplashMessage(string message, bool itWasrinvoked = false)
    {
      var launched = false;
      while (!launched &&
             !itWasrinvoked)
        lock (locker)
        {
          if (!WaitPlease)
            launched = true;
        }

      if (_splashForm   == null ||
          _splashThread == null)
        return;

      if (_splashForm.InvokeRequired)
      {
        SplashHandler handle = SplashMessage;
        _splashForm.Invoke(handle, message, true);
        return;
      }

      _splashForm.Message = message;
    }

    /// <summary>
    ///   The splash show.
    /// </summary>
    public static void SplashShow()
    {
      if (_splashForm != null)
        return;

      var t = Task.Factory.StartNew(obj => ShowSplashThreadCall(), null);
      t.Wait();
    }

    /// <summary>
    ///   The show splash thread call.
    /// </summary>
    private static void ShowSplashThreadCall()
    {
      if (_splashForm == null)
        _splashForm = new Processing();

      _splashForm.TopMost = true;
      _splashForm.Show();

      lock (locker)
      {
        WaitPlease = false;
      }

      Application.Run(_splashForm);
    }

    /// <summary>
    ///   The splash handler.
    /// </summary>
    /// <param name="message">
    ///   The message.
    /// </param>
    /// <param name="itWasRinvoked">
    ///   The it was rinvoked.
    /// </param>
    private delegate void SplashHandler(string message, bool itWasRinvoked);
  }
}