   M S B A M L   `   `   `  ���� 
  Kardex    SiasoftAppExt.Kardex   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    >d2http://schemas.microsoft.com/expression/blend/2008    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     a ]Syncfusion.SfGrid.WPF, Version=16.1460.0.32, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89j fSyncfusion.SfGridConverter.WPF, Version=16.1460.0.32, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89` \Syncfusion.Tools.Wpf, Version=16.1460.0.32, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89_ [Syncfusion.Grid.Wpf, Version=16.1460.0.32, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89a	 ]Syncfusion.Shared.Wpf, Version=16.1460.0.32, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89:
syncfusion!http://schemas.syncfusion.com/wpf     	 $	��600��$
��1000��  � 	Resources  3  -Syncfusion.UI.Xaml.Grid.GridHeaderCellControl �� 
TargetType   headerStyle �� Property �� Value��%y   &	        �� "  .���  � ��$
 Bold"���  � ��$ 12(���  � ��
 �������  � �� �#bdc3c7.��� 
������� � RowDefinitions �� $��40 ��� $��40 ��� $��1* �.+�� $��0��$��0,-2,0,2q� � ColumnDefinitions �� $��1* ��� $��1* ��� $��1* ��� $��1* �.+�� $��0�� �� $
��Auto ��� $
��0.5* ��� $��1* �.+��� $��Codigo: ��$��
10 0 10 0 q�$��Right=�$��Center=��� -    ��Name$ 
TextBoxRef��$��1��$��5q�
 � Tag$ 	inmae_ref���� -   $��TXNomRef��$��2��$��5q� �� 
IsReadOnly . � $��1�� �� $
��Auto ��� $
��0.5* ��� $��1* �.+��� $��Bodega: ��$��
10 0 10 0 q�$��Right=�$��Center=��� -   $ 
TextBoxbod��$��1��$��5q�$ 	inmae_bod���� -   $��TxNomBod��$��2��$��5q� . � $��2�� �� $
��Auto ��� $��1* �.+��� $��Fecha Corte: ��$��
10 0 10 0 q�$��Right=�$��Center=�(  "System.Windows.Controls.DatePicker  -   $��FecIni��$��4��$��Center=�$��	5 0 10 0 q�� $��3�� �� $��1* ��� $��1* �.+��� -   $��0��$��5q�
������	 � Cursor$
	 Hand��.��	Consultar�� -   $��1��$��5q�
������$
	 Hand��.��Excel� $��1�� �� $��1* ��� $��1* ��� $��1* ��� $��1* �.+�� $��0�� �� $
��Auto ��� $��1* �.+��� $��TRN: ��$��
10 0 10 0 q�$��Right=�$��Center=��� $��1��$��5q��� $��2��$��5q�(  "Syncfusion.UI.Xaml.Grid.SfDataGrid  -   
  Name$
 
GridKardex��$��2��(  "Syncfusion.UI.Xaml.Grid.SfGridBase   FrozenColumnCount$ 2��   AllowResizingColumns .    EnableDataVirtualization .    AutoGenerateColumns .     ShowGroupDropArea . $��12(�   AllowFiltering .    HeaderStyle# [     Columns ,  &Syncfusion.UI.Xaml.Grid.GridTextColumn  ,  &Syncfusion.UI.Xaml.Grid.GridColumnBase   
HeaderText$	 Trn��   MappingName$ cod_trn��   Width$ 70Q�   TextAlignment$
 Left=�  $ 	Documento��$ num_trn��$	 140Q�$
 Left=�  $ Fecha��$ fec_trn��$	 200Q�$
 Left=�  $ BodTras��$ bod_tra��$ 70Q�$
 Left=�  $ Unidades��$ ent_uni��$ 70Q�$
 Left=�   	CellStyle �� &   Syncfusion.UI.Xaml.Grid.GridCell"  .���  � �� �	#3316A085  $ CostUnit��$ Right=�$	 100Q�   DisplayBinding ��  �� Path$ ent_cost� �� StringFormat$ {0:c}�� �� "  .���  � �� �	#3316A085  $ 	CostTotal��$ Right=�$	 100Q� �� $ 
ent_ctotal�$ {0:c}�� �� "  .���  � �� �	#3316A085  $ Unidades��$ sal_uni��$ 70Q�$ Right=� �� "  .���  � �� �	#33F1C40F  $ CostUnit��$ Right=�$	 100Q� �� $ sal_cost�$ {0:c}�� �� "  .���  � �� �	#33F1C40F  $ 	CostTotal��$ Right=�$	 100Q� �� $ 
sal_ctotal�$ {0:c}�� �� "  .���  � �� �	#33F1C40F  $ Unidades��$ 	saldo_uni��$ 70Q�$ Right=� �� "  .���  � �� �	#332980B9  $ CostUnit��$ Right=�$	 100Q� �� $ 
saldo_cost�$ {0:c}�� �� "  .���  � �� �	#332980B9  $ 	CostTotal��$ Right=�$	 100Q� �� $ saldo_ctotal�$ {0:c}�� �� "  .���  � �� �	#332980B9