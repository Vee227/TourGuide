   M S B A M L   `   `   `  ����D  @TourGuide, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:TourGuide.PresentationLayer.Views!TourGuide.PresentationLayer.Views  5   	   6    0TourGuide.PresentationLayer.Views.AddTourLogView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Alocal/clr-namespace:TourGuide.PresentationLayer.Views     5   	     � Title$  AddTourLogView��5   	   $	��480��6    $	��420��6-   .��5	      � $��20q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��Add Tour Log��6   $��20(�6(   $
��Bold"�66   $��Center=�5      $��0��61   $��0,0,0,20q�6>   �� 5   
   $��1��6   $��0,0,0,10q�6"   .+�5      �� $��Date (YYYY-MM-DD):��6   $��SemiBold"�62   �� 5      -   5         ��Name$ DateTextBox��5      $	��360��6+   $��30��67   $��5q�6C   5      �� 5   
   $��2��6   $��0,0,0,10q�6"   .+�5      �� $��Comment:��6   $��SemiBold"�6(   �� 5      -   5        $ CommentTextBox��5      $	��360��6.   $��60��6:   $��5q�6F    �� TextWrapping$
 Wrap=�6R   5      �� 5!   
   $��3��6   $��0,0,0,10q�6"   .+�5"      �� $��Difficulty (1-5):��6   $��SemiBold"�61   �� 5#      -   5         ��Name$ DifficultyComboBox��5#      $	��360��63   $��35��6?   $��5q�6K   .@�5$      �� $��1��6   �� 5%      $��2��6   �� 5&      $��3��6   �� 5'      $��4��6   �� 5(      $��5��6   5)      5*      �� 5,   
   $��4��6   $��0,0,0,10q�6"   .+�5-      �� $��Total Time (minutes):��6   $��SemiBold"�65   �� 5.      -   5        $ TotalTimeTextBox��5.      $	��360��60   $��30��6<   $��5q�6H   5/      �� 51   
   $��5��6   $��0,0,0,10q�6"   .+�52      �� $��Rating (1-5):��6   $��SemiBold"�6-   �� 53      -   5        $ RatingComboBox��53      $	��360��6/   $��35��6;   $��5q�6G   .@�54      �� $��1��6   �� 55      $��2��6   �� 56      $��3��6   �� 57      $��4��6   �� 58      $��5��6   59      5:      �� 5<   
   $��6��6   $��Center=�6"   $��
20,10,20,0q�6?   .+�5=      �� -   5        $��💾 Save Log��5=      ���#27AE60
�������$	��180��5?      $��40��6!   $��14(�6-    � Cursor$
 Hand��5@      $
��Bold"�6#   5A      5C      5D      