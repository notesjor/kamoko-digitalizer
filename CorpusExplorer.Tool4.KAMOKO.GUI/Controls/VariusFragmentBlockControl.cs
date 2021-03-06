﻿#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CorpusExplorer.Tool4.KAMOKO.GUI.Controls.Abstract;
using CorpusExplorer.Tool4.KAMOKO.GUI.Properties;
using CorpusExplorer.Tool4.KAMOKO.Model;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment;
using CorpusExplorer.Tool4.KAMOKO.Model.Fragment.Abstract;

#endregion

namespace CorpusExplorer.Tool4.KAMOKO.GUI.Controls
{
  public partial class VariusFragmentBlockControl : AbstractFragmentControl
  {
    private readonly VariableFragment _fragment;

    private bool _isReadOnly;

    public VariusFragmentBlockControl(AbstractFragment fragment, bool isReadOnly)
    {
      _fragment = fragment as VariableFragment;

      InitializeComponent();
      IsReadOnly = isReadOnly;

      LoadSentence();
    }

    public bool IsReadOnly
    {
      get => _isReadOnly;
      set
      {
        _isReadOnly = value;
        btn_item_remove.Visible = btn_item_add.Visible = !value;
      }
    }

    public event EventHandler FragmentSubAdd;

    public override AbstractFragment GetFragment()
    {
      SaveSentence();
      var list = new List<AbstractFragment>();
      for (var i = radScrollablePanel1.PanelContainer.Controls.Count - 1; i > -1; i--)
      {
        var afc = radScrollablePanel1.PanelContainer.Controls[i] as AbstractFragmentControl;
        if (afc != null)
          list.Add(afc.GetFragment());
      }

      return new VariableFragment {Index = _fragment.Index, Fragments = list};
    }

    private void btn_item_add_const_Click(object sender, EventArgs e)
    {
      btn_item_add.HideDropDown();
      SaveSentence();
      OnAddConstnat(_fragment);
    }

    private void btn_item_add_vars_Click(object sender, EventArgs e)
    {
      btn_item_add.HideDropDown();
      SaveSentence();
      OnAddVariable(_fragment);
    }

    private void btn_item_remove_Click(object sender, EventArgs e)
    {
      SaveSentence();
      OnDelete(_fragment);
    }

    private void control_FragmentSubAdd(object sender, EventArgs e)
    {
      SaveSentence();
      FragmentSubAdd?.Invoke(null, null);
    }

    private void ControlOnFragmentAddConstant(AbstractFragment fragment)
    {
      SaveSentence();
      _fragment.Fragments.Add(new ConstantFragment {Content = "", Index = -1, SpeakerVotes = new List<SpeakerVote>()});
      LoadSentence();
      FragmentSubAdd?.Invoke(null, null);
    }

    private void ControlOnFragmentAddVariable(AbstractFragment fragment)
    {
      SaveSentence();
      _fragment.Fragments.Add(
                              new VariableFragment
                              {
                                Fragments =
                                  new List<AbstractFragment>
                                  {
                                    new ConstantFragment
                                    {
                                      Content = "",
                                      Index = 1,
                                      SpeakerVotes =
                                        new List<SpeakerVote>()
                                    }
                                  }
                              });
      LoadSentence();
      FragmentSubAdd?.Invoke(null, null);
    }

    private void ControlOnFragmentDelete(AbstractFragment fragment)
    {
      SaveSentence();

      var idx = _fragment.Fragments.FindIndex(x => x.Index == fragment.Index);
      if (idx == -1)
        return;

      if (_fragment.Fragments.Count < 2)
      {
        MessageBox.Show(
                        Resources.VariusFragmentBlockControl_VarDelteNotAccepted,
                        Resources.VariusFragmentBlockControl_VarDelteNotAcceptedHead,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
        return;
      }

      _fragment.Fragments.RemoveAt(idx);
      LoadSentence();
      FragmentSubAdd?.Invoke(null, null);
    }

    private void LoadSentence()
    {
      radScrollablePanel1.Controls.Clear();

      for (var i = _fragment.Fragments.Count - 1; i > -1; i--)
      {
        var fragment = _fragment.Fragments[i];
        if (fragment is ConstantFragment)
        {
          var control = new ConstantFragmentBlockControl(fragment, IsReadOnly) {Dock = DockStyle.Top};
          control.FragmentAddConstant += ControlOnFragmentAddConstant;
          control.FragmentAddVariable += ControlOnFragmentAddVariable;
          control.FragmentDelete += ControlOnFragmentDelete;

          radScrollablePanel1.PanelContainer.Controls.Add(control);
        }
        else if (fragment is VariableFragment)
        {
          var control = new VariusFragmentBlockControl(fragment, IsReadOnly) {Dock = DockStyle.Top};
          control.FragmentAddConstant += ControlOnFragmentAddConstant;
          control.FragmentAddVariable += ControlOnFragmentAddVariable;
          control.FragmentDelete += ControlOnFragmentDelete;
          control.FragmentSubAdd += control_FragmentSubAdd;

          radScrollablePanel1.PanelContainer.Controls.Add(control);
        }
        else
        {
          throw new StrongTypingException();
        }
      }
    }

    private void SaveSentence()
    {
      var fragments = new List<AbstractFragment>();
      for (var i = radScrollablePanel1.PanelContainer.Controls.Count - 1; i > -1; i--)
      {
        var afc = radScrollablePanel1.PanelContainer.Controls[i] as AbstractFragmentControl;
        if (afc == null)
          continue;
        fragments.Add(afc.GetFragment());
      }

      _fragment.Fragments = fragments;
    }
  }
}