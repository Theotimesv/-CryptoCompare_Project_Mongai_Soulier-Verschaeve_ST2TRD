   M S B A M L   `   `   `  ����P  LCryptoCompare_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:CryptoCompare_Project.ViewsCryptoCompare_Project.Views  5      ,    &CryptoCompare_Project.Views.Variations   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QScottPlot.WPF, Version=4.1.33.0, Culture=neutral, PublicKeyToken=e53b06131e34a3aaR NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:CryptoCompare_Project.Views     5      ���	#FF273655.��5	      �� $
��Left=�6   $	��100��6(   $	��Top=�65   $	��100��6M   .+�5
   
   �� $	��500��6   
  �� Top$  102.444��6"   $	��170��67    �� Left$ 0.667��6C   ���	#FF353F4E.+�5      �� $��Homepage��5      
�������$��Center=�5      $��Center=�5      $	��170��5      $��40��6!   ���	#FF353F4E���	#FF273655��5      �� *GotoHomePageCommand+5   D   �� 5      $��Crypto Rates��5      
�������$��Center=�5      $��Center=�5      $	��170��5      $��40��6!   ���	#FF353F4E���	#FF273655$��0q�6\   ��5      �� *GotoCryptoRatesCommand+5   \   �� 5      $��
Variations��5      
�������$��Center=�5      $��Center=�5      $	��170��5       $��40��6!   ���	#FF353F4E���	#FF273655��5      �� *GotoVariationsCommand+5    D   �� 5!      $��Notifications��5"      
�������$��Center=�5%      $��Center=�5&      $	��170��5'      $��40��6   ���	#FF353F4E���	#FF273655��5$      �� *GotoNotificationsCommand+5'   @   5(      �� 5)   
   $	��100��6   $  2��6"   $	��900��61   ���	#FF353F4E$��Center=�6T   $��Center=�6q   .+�5*      �� $��myCrypto��6   $��Center=�6'   $��24(�6D   $��Impact)�6R   $��394.833,0,394.197,0q�6f   $��Center=�6�   $��110.97��6�   
�������6�   �� 5+      $��
Variations��6   $��Center=�6)   $��16(�6F   $��401.833,0,402.197,0q�6T   $��95.97��6q   $
��Bold"�6   
�������6�   5,      �� 5-   
   $	��500��6   $  99��6"   $	��170��62   $	 730��6>   ���	#FF353F4E6P   � 5.   
   $ 212.333��6   $  138.666��6%   $	��427��6:   $��479.222��6G    � ColumnDefinitions 5/      �� 50      $��5* �6#   �� 51      $��5* �6#    � RowDefinitions 53      �� 54      $	��25* �6    �� 55      $	��25* �6    �� 56      $	��25* �6    �� 57      $	��25* �6    �� 58      $	��25* �6    .+�5<      �� $��0��6   $��0��6&   $��50��63   $	��150��6?   $��20(�6K   $��Center=�6Y   $��Center=�6v   .��6�   
Currency5>      �� 5@      -   5        $��Crypto1��5@   4   $��0��6   $��1��6'   $��50��6C   $	��150��6O   $��20(�6[   $��Center=�6i   $��Center=�6�    �� SelectedIndex$ 0��6�   .@�5A      �� $	��BTC��6   �� 5B      $	��ETH��6   �� 5C      $
��USDT��6   �� 5D      $	��BNB��6   �� 5E      $
��USDC��6   �� 5F      $	��ADA��6   �� 5G      $	��SOL��6   �� 5H      $	��XRP��6   �� 5I      $
��LUNA��6   �� 5J      $	��DOT��6   �� 5K      $
��DOGE��6   �� 5L      $
��AVAX��6   5M      �� 5O      -   5        $��Crypto2��5O   4   $��0��6   $��2��6'   $��50��6C   $	��150��6O   $��20(�6[   $��Center=�6i   $��Center=�6�   $ 0��6�   .@�5P      �� $	��ETH��6   �� 5Q      $	��BTC��6   �� 5R      $
��USDT��6   �� 5S      $	��BNB��6   �� 5T      $
��USDC��6   �� 5U      $	��ADA��6   �� 5V      $	��SOL��6   �� 5W      $	��XRP��6   �� 5X      $
��LUNA��6   �� 5Y      $	��DOT��6   �� 5Z      $
��DOGE��6   �� 5[      $
��AVAX��6   5\      �� 5^      $��0��6   $��3��6&   $��50��63   $	��150��6?   $��20(�6K   $��Center=�6Y   $��Center=�6v   .��6�   Delta5`      �� 5b      -   5        $��Period��5b   F   $��0��6   $��4��6'   $��0,20,0,0q�64   $��50��6T   $	��150��6`   $��20(�6l   $��Center=�6z   $��Center=�6�   $ 0��6�   .@�5c      �� $��Today��6   �� 5d      $��1 hour��6   �� 5e      $��24 hours��6   �� 5f      $��7 days��6   �� 5g      $��30 days��6   �� 5h      $��90 days��6   �� 5i      $��1 year��6   5j      �� 5l      -   5        $��
ColumnName��5l   3   $��1��6   $��0��6&   $��50��6E   $	��150��6Q   $��20(�6]   .��6k   	Variation5n      �� 5p      -   5        $��Crypto1Variation��5p   3   $��1��6   $��1��6&   $��50��6K   $	��150��6W   $��20(�6c   �� 5r      -   5        $��Crypto2Variation��5r   3   $��1��6   $��2��6&   $��50��6K   $	��150��6W   $��20(�6c   �� 5t      -   5        $��Delta��5t   3   $��1��6   $��3��6&   $��50��6@   $	��150��6L   $��20(�6X    �� 
IsReadOnly . 6f   5u      5v      5w      