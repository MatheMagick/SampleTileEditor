﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GameLogic.Data;
using UbisoftTileEditor.Annotations;
using UbisoftTileEditor.ViewModels.Data;

namespace UbisoftTileEditor
{
    /// <summary>
    /// Interaction logic for WorldViewControl.xaml
    /// </summary>
    public partial class WorldViewControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty GameWorldProperty = DependencyProperty.Register("GameWorld", typeof(GameWorldViewModel), typeof(WorldViewControl));
        private byte _selectedTemplateIndex;

        public WorldViewControl()
        {
            InitializeComponent();
            this.ChangeTemplateCommand = new RelayCommand(x => this.ExecuteChangeTemplate(x));
        }

        public byte SelectedTemplateIndex
        {
            get { return _selectedTemplateIndex; }
            set
            {
                _selectedTemplateIndex = value; 
                this.OnPropertyChanged();
            }
        }

        public GameWorldViewModel GameWorld
        {
            get
            {
                return this.GetValue(GameWorldProperty) as GameWorldViewModel;
            }
            set
            {
                this.SetValue(GameWorldProperty, value);
            }
        }

        public ICommand ChangeTemplateCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void WorldViewControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            RegenerateWorld();
        }

        private void RegenerateWorld()
        {
            this.PanelTiles.Children.Clear();
            var worldSize = this.GameWorld.WorldSize;

            for (byte y = 0; (y*worldSize.TileHeight) < this.PanelTiles.ActualHeight; y++)
            {
                for (byte x = 0; (x*worldSize.TileWidth) < this.PanelTiles.ActualWidth; x++)
                {
                    var image = new Image();
                    image.Width = worldSize.TileWidth;
                    image.Height = worldSize.TileWidth;

                    if ((y < worldSize.HeightInTiles) && (x < worldSize.WidthInTiles))
                    {
                        Binding imageSourceBinding =
                            new Binding(string.Format("GameWorld[{0},{1}].TemplateIndex", x.ToString(), y.ToString()));
                        imageSourceBinding.Converter = new CellToBitmapImageConverter(GameWorld);
                        image.SetBinding(Image.SourceProperty, imageSourceBinding);
                    }

                    Button buton = new Button();
                    buton.BorderThickness = new Thickness(0);
                    buton.Style = (Style) this.FindResource(ToolBar.ButtonStyleKey);
                    buton.Margin = new Thickness(0);
                    buton.Padding = new Thickness(0);
                    buton.Content = image;
                    buton.CommandParameter = new Vector(x, y);
                    buton.Command = this.ChangeTemplateCommand;
                    this.PanelTiles.Children.Add(buton);
                }
            }
        }

        private void ExecuteChangeTemplate(object o)
        {
            Vector position = (Vector)o;

            byte x = Convert.ToByte(position.X);
            byte y = Convert.ToByte(position.Y);

            var cell = this.GameWorld.Cells.FirstOrDefault(c => c.TileX == x && c.TileY == y);

            if (cell == null)
            {
                cell = new CellViewModel(new Cell(){ TileX = x, TileY = y });
                this.GameWorld.Cells.Add(cell);
            }

            //cell.TemplateIndex = this.SelectedTemplateIndex;
            cell.TemplateIndex = (byte)this.ListBox.SelectedIndex;
        }
    }
}