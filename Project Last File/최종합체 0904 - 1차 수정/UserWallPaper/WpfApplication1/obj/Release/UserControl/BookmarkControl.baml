   M S B A M L   `   `   `  ����   UserWallPaper#    UserWallPaper.BookmarkControl   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -     � 	Resources     buttonstyle �� 
TargetType �� Property �� Value �� 
TargetType �� Padding  ButtonFocusVisual J� SnapsToDevicePixels �� StrokeDashArray  ButtonNormalBackgroundFill �� EndPoint	 �� 
StartPoint
 � Color � Offset  ColorButtonStyleFirstRow ��Name �� Triggers P� Property P� Value �� 
TargetName��%�  &	        &	 �     &	     &	 p    7  7  �� " d�.���  �@�OverridesDefaultStyle . ��  � ��$ 0q���  � �� �� " d�.R��� $��border��$��1q�$	 4,2q�
������ 
������ ._��� $��Center=�$��Center=��� .���  � �� �� .R���  . $��3q�
���   �$	 1 2R�$��1���� $ 0.5,1&�$	 0.5,0&�.��� $
 #EDEDED��$ 0Q�� $
 #EDEDED��$	 0.9Q��� " ��.���  �@�FocusVisualStyle8   ��  � ��8  ��  � ��# � ����  � ��$ Center=���  � ��$ Center=���  � �� �� " ��.R��� $ buttonBorder��$��13��$��13��$��1q�$��0q����#D0D0D0#��z��._��� $ innerBorder��$��12��$��12��$	��0.5q�$��Center=�$��Center=���Z�  P�  �@J�IsMouseOver . . ��� $ buttonBorder�� � ��$ 2q��� $ buttonBorder�� � �� �#FEAB38�� $ innerBorder�� � �� �#FFFFFF.��� $��0q�.+��� -    ��Name$ image������  �� Path$ Width� �� ElementName$
 user�� �� Mode$ TwoWay=����� $ Height�$
 user��$ TwoWay=�d� -    d�Name$ myToggle��$��0q�
������ #���   ���� $ Width�$
 user��$ TwoWay=����� $ Height�$
 user��$ TwoWay=�� -    �Name$ Popup��$��Bottom=� � AllowsTransparency . |�.  $
��Fade=� � 	StaysOpen .   � PlacementTarget �� $ myToggle��.���� .+��� -   $��listview��$	��100��$	��500��$	��450�� �� View �� .P���  �� DisplayMemberBinding �� *Name+.��교재이름��  �� *Page+.��	페이지��  �� Width$	 250�� �� *Memo+.��메모 � ContextMenu �� .@�m� -   $��북마크 제거��m� -   $��북마크 모두 제거��]� $��Bottom=�.+�d� -   $ addtogle��$��북마크 추가��$��80��$��Right=�� -	   $ addpopup��$��Bottom=� . |�.  $
��Fade=� .   �� $ addtogle��.��� $��63��$	��198��.+��� -
    ��Name$
 memo��$	��198��$��25��$�� ��$	��Top=��� -   $��추가��$
��Left=�$��28��$��	99,35,0,0q�$	��Top=�$��47���� -   $��취소��$
��Left=�$��28��$��
151,35,0,0q�$	��Top=�$��47��