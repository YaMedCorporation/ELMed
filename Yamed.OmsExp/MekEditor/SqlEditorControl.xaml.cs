﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using Yamed.Entity;
using Yamed.Server;

namespace Yamed.OmsExp.MekEditor
{
    /// <summary>
    /// Логика взаимодействия для SqlEditorControl.xaml
    /// </summary>
    public partial class SqlEditorControl : UserControl
    {
        public SqlEditorControl()
        {
            InitializeComponent();
            //var resourceNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();

            OsnBoxEdit.DataContext = SprClass.Otkazs;

            using (
                var stream =
                    System.Reflection.Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("Yamed.OmsExp.MekEditor.SqlSyntax.xshd"))
            {
                using (var reader = new System.Xml.XmlTextReader(stream))
                {
                    sqlEditor.SyntaxHighlighting =
                                        ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader,
                            ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);

                    sqlEditor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
                    sqlEditor.TextArea.TextEntered += textEditor_TextArea_TextEntered;

                }
            }

            MekList.DataContext = SqlReader.Select("Select * From Yamed_ExpSpr_ExpAlg where ExpType = 1", SprClass.LocalConnectionString);

        }

        CompletionWindow completionWindow;

        void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ".")
            {
                // Open code completion after the user has pressed dot:
                completionWindow = new CompletionWindow(sqlEditor.TextArea);
                IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                data.Add(new MyCompletionData("Item1"));
                data.Add(new MyCompletionData("Item2"));
                data.Add(new MyCompletionData("Item3"));
                completionWindow.Show();
                completionWindow.Closed += delegate {
                    completionWindow = null;
                };
            }
        }

        void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }

        private void SqlEditor_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                SqlExecute.GetData(GridControl1, sqlEditor.Text);
            }
        }

        private void SqlEditorControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            ((IList) GridControl1.DataContext)?.Clear();
        }
    }

    /// Implements AvalonEdit ICompletionData interface to provide the entries in the
    /// completion drop down.
    public class MyCompletionData : ICompletionData
    {
        public MyCompletionData(string text)
        {
            this.Text = text;
        }

        public System.Windows.Media.ImageSource Image
        {
            get { return null; }
        }

        public string Text { get; private set; }

        // Use this property if you want to show a fancy UIElement in the list.
        public object Content
        {
            get { return this.Text; }
        }

        public object Description
        {
            get { return "Description for " + this.Text; }
        }

        public double Priority { get; }

        public void Complete(TextArea textArea, ISegment completionSegment,
            EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }
    }
}
