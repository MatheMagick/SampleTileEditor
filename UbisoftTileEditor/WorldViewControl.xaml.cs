﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GameLogic.Data;
using UbisoftTileEditor.Annotations;
using UbisoftTileEditor.Converters;
using UbisoftTileEditor.Helpers;
using UbisoftTileEditor.ViewModels.Data;

namespace UbisoftTileEditor
{
    /// <summary>
    /// Interaction logic for WorldViewControl.xaml
    /// </summary>
    public partial class WorldViewControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty GameWorldProperty = DependencyProperty.Register("GameWorld", typeof(GameWorldViewModel), typeof(WorldViewControl), new PropertyMetadata(GameWorldChanged));

        private static void GameWorldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var worldViewControl = (WorldViewControl)d;

            if (e.OldValue != null)
            {
                ( (GameWorldViewModel)e.OldValue ).WorldSize.PropertyChanged -= worldViewControl.WorldSize_PropertyChanged;
                ( (GameWorldViewModel)e.OldValue ).PropertyChanged -= worldViewControl.World_PropertyChanged;
            }

            worldViewControl.RegenerateWorld();
            worldViewControl.ShowGameObjects();

            if (e.NewValue != null)
            {
                ( (GameWorldViewModel)e.NewValue ).WorldSize.PropertyChanged += worldViewControl.WorldSize_PropertyChanged;
                ( (GameWorldViewModel)e.NewValue ).PropertyChanged += worldViewControl.World_PropertyChanged;
            }
        }

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

        private void WorldSize_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // No need to redisplay the game objects, since they use absolute coordinates
            this.RegenerateWorld();
        }

        private void World_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("DefaultCellTemplateIndex", StringComparison.OrdinalIgnoreCase))
            {
                this.RegenerateWorld();
            }
        }

        private void RegenerateWorld()
        {
            // TODO Turn this into pure MVVM
            this.PanelTiles.Children.Clear();

            var worldSize = this.GameWorld.WorldSize;
         
            if (worldSize.TileHeight > 0 && worldSize.TileWidth > 0 && worldSize.HeightInTiles > 0 && worldSize.WidthInTiles > 0)
            {
                for (byte y = 0; ( ( y + 1 ) * worldSize.TileHeight ) <= this.PanelTiles.Height; y++)
                {
                    for (byte x = 0; ( ( x + 1 ) * worldSize.TileWidth ) <= this.PanelTiles.Width; x++)
                    {
                        var image = new Image();
                        image.Width = worldSize.TileWidth;
                        image.Height = worldSize.TileHeight;
                        image.Stretch = Stretch.None;

                        if (( y < worldSize.HeightInTiles ) && ( x < worldSize.WidthInTiles ))
                        {
                            Binding imageSourceBinding = new Binding(string.Format("GameWorld[{0},{1}].TemplateIndex", x.ToString(), y.ToString()));
                            imageSourceBinding.Mode = BindingMode.OneWay;
                            imageSourceBinding.Converter = new TemplateIndexToBitmapImageConverter(GameWorld.Templates);
                            image.SetBinding(Image.SourceProperty, imageSourceBinding);
                        }

                        Button button = new Button();

                        // Remove border and highlight style
                        button.BorderThickness = new Thickness(0);
                        button.Style = (Style)this.FindResource(ToolBar.ButtonStyleKey);
                        button.Padding = new Thickness(0);

                        button.Width = worldSize.TileWidth;
                        button.Height = worldSize.TileHeight;
                        button.Content = image;
                        button.CommandParameter = new Vector(x, y);
                        button.Command = this.ChangeTemplateCommand;

                        this.PanelTiles.Children.Add(button);
                    }
                }
            }

            ShowGameObjects();
        }

        private void ShowGameObjects()
        {
            this.PanelGameObjects.Children.Clear();

            foreach (var gameObject in this.GameWorld.GameObjects)
            {
                DisplayGameObject(gameObject);
            }
        }

        private void ExecuteChangeTemplate(object o)
        {
            if (!this.GameWorld.IsInAddMode)
            {
                var mouse = Mouse.GetPosition(this.PanelTiles);

                var gameObjectViewModel = this.GameWorld.GameObjects.FirstOrDefault(x => IsMouseInGameObject(mouse, x));

                if (gameObjectViewModel != null)
                {
                    this.GameWorld.GameObjects.Remove(gameObjectViewModel);
                    var gameObjectDisplayImage = this.PanelGameObjects.Children.Cast<Image>().First(x => x.DataContext == gameObjectViewModel);
                    this.PanelGameObjects.Children.Remove(gameObjectDisplayImage);
                }
            }
            else if (this.GameWorld.SelectedTemplateIndex < 4)
            {
                Vector position = (Vector)o;

                byte x = Convert.ToByte(position.X);
                byte y = Convert.ToByte(position.Y);

                var cell = this.GameWorld.Cells.FirstOrDefault(c => c.TileX == x && c.TileY == y);

                if (cell == null)
                {
                    cell = new CellViewModel(new Cell() { TileX = x, TileY = y });
                    this.GameWorld.Cells.Add(cell);
                }

                cell.TemplateIndex = this.GameWorld.SelectedTemplateIndex;
            }
            else
            {
                var mouse = Mouse.GetPosition(this.PanelTiles);

                var gameObjectViewModel = new GameObjectViewModel(new GameObject()) { TemplateIndex = this.GameWorld.SelectedTemplateIndex, X = Convert.ToInt32(mouse.X), Y = Convert.ToInt32(mouse.Y) };

                this.GameWorld.GameObjects.Add(gameObjectViewModel);

                DisplayGameObject(gameObjectViewModel);
            }
        }

        private bool IsMouseInGameObject(Point mouse, GameObjectViewModel gameObject)
        {
            var templateComponent = this.GameWorld.Templates[gameObject.TemplateIndex].Components[0];
            var gameObjectRect = new Rect(new Point(gameObject.X - templateComponent.Width / 2, gameObject.Y - templateComponent.Height / 2), new Vector(templateComponent.Width, templateComponent.Height));

            return gameObjectRect.Contains(mouse);
        }

        private void DisplayGameObject(GameObjectViewModel gameObjectViewModel)
        {
            var image = new Image();
            var template = this.GameWorld.Templates[gameObjectViewModel.TemplateIndex];
            image.DataContext = gameObjectViewModel;
            image.Width = template.Components[0].Width;
            image.Height = template.Components[0].Height;
            image.Opacity = template.Components[0].Alpha;
            image.RenderTransform = new RotateTransform(( template.Angle / Math.PI ) * 180, image.Width / 2, image.Height / 2);
            image.Source = template.BitmapImage;
            image.Margin = new Thickness(gameObjectViewModel.X - image.Width / 2, gameObjectViewModel.Y - image.Height / 2, 0, 0);
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.VerticalAlignment = VerticalAlignment.Top;
            this.PanelGameObjects.Children.Add(image);
        }
    }
}