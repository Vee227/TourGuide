   M S B A M L   `   `   `  ����D  @TourGuide, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:TourGuide.PresentationLayer.Views!TourGuide.PresentationLayer.Views  5   	   9    3TourGuide.PresentationLayer.Views.ModifyTourLogView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Alocal/clr-namespace:TourGuide.PresentationLayer.Views     5   	     � Title$  ModifyTourLogView��5   	   $	��450��6#   $	��400��60   .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��Date:��6   $��0��6!   $��0,5,0,0q�6.   �� 5   
   $��1��6E   $��Stretch=�6R   $��0,2,0,8q�6p   $��5q�6�   ��6   �� *SelectedTourLog.Date+ �� Mode$ TwoWay=�6�   �� 5   
   $��Comment:��6   $��2��6$   $��0,5,0,0q�61   �� 5   
   $��3��6H   $��Stretch=�6U   $��0,2,0,8q�6s   $��5q�6�   ��6   �� *SelectedTourLog.Comment+$ TwoWay=�6�   �� 5    
   $��Difficulty (1-5):��6   $��4��6-   $��0,5,0,0q�6:   �� 5!   
   -   5         ��Name$ DifficultyComboBox��5!      $��5��6/   $��Stretch=�6<   $��0,2,0,8q�6�   $��5q�6�    �� SelectedItem 6Z   �� *SelectedTourLog.Difficulty+$ TwoWay=�.@�5"      �� $��1��6   �� 5#      $��2��6   �� 5$      $��3��6   �� 5%      $��4��6   �� 5&      $��5��6   5'      �� 5)   
   $��Total Time (min):��6   $��6��6-   $��0,5,0,0q�6:   �� 5*   
   -   5         ��Name$ TotalTimeTextBox��5*      $��7��6d   $��Stretch=�6q   $��0,2,0,8q�6�   $��5q�6�   ��6,   �� *SelectedTourLog.TotalTime+$ TwoWay=�6�   �� 5,   
   $��Rating (1-5):��6   $��8��6)   $��0,5,0,0q�66   �� 5-   
   -   5        $ RatingComboBox��5-      $��9��6+   $��Stretch=�68   $��0,2,0,8q�6�   $��5q�6�    6V   �� *SelectedTourLog.Rating+$ TwoWay=�.@�5.      �� $��1��6   �� 5/      $��2��6   �� 50      $��3��6   �� 51      $��4��6   �� 52      $��5��6   53      �� 55   
   -   5        $��	💾 Save��55      $��10��6#   $��Center=�6K   $	��100��6h   $��8q�6t   $��0,10,0,0q�6�   ���#27AE60
�������6�   57      58      