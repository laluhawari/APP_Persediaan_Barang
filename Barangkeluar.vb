﻿Public Class Barangkeluar
    Sub tampilgrid()
        Call Koneksi()
        DA = New OleDb.OleDbDataAdapter("SELECT * FROM tbbarangkeluar", CONN)
        DS = New DataSet
        DA.Fill(DS)
        Grid2.DataSource = DS.Tables(0)
        Grid2.ReadOnly = True
    End Sub
    Sub kosongkanfield()
        txtidtransaksi.Text = ""
        cbkodebarang.Text = ""
        txtnamabarang.Text = ""
        txtpersediaan.Text = ""
        txtjumlah.Text = ""
    End Sub
    Sub matikanfield()
        txtidtransaksi.Enabled = False
        datetgl.Enabled = False
        cbkodebarang.Enabled = False
        txtnamabarang.Enabled = False
        txtpersediaan.Enabled = False
        txtjumlah.Enabled = False
    End Sub
    Sub hidupkanfield()
        txtidtransaksi.Enabled = True
        datetgl.Enabled = True
        cbkodebarang.Enabled = True
        txtnamabarang.Enabled = True
        txtpersediaan.Enabled = True
        txtjumlah.Enabled = True
    End Sub
    Sub simpandata()
        Call Koneksi()
        DA = New OleDb.OleDbDataAdapter("SELECT * FROM tbbarang", CONN)
        DS = New DataSet
        DA.Fill(DS)
        RECORD.DataSource = DS
        RECORD.DataMember = DS.Tables(0).ToString
    End Sub
    Sub itemcombo()
        Call Koneksi()
        DA = New Data.OleDb.OleDbDataAdapter("SELECT * FROM tbbarang", CONN)
        DS = New DataSet
        DA.Fill(DS)
        RECORD.DataSource = DS
        RECORD.DataMember = DS.Tables(0).ToString
        Dim a As DataRow
        cbkodebarang.Items.Clear()
        For Each a In DS.Tables(0).Rows
            cbkodebarang.Items.Add(a.Item(0))
        Next a
    End Sub
    Sub RefreshGrid()
        DA = New OleDb.OleDbDataAdapter("SELECT * FROM tbbarangkeluar", CONN)
        DS = New DataSet
        DS.Clear()
        DA.Fill(DS, "tbbarangkeluar")
        Grid2.DataSource = (DS.Tables("tbbarangkeluar"))
    End Sub
    Private Sub Barangkeluar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call tampilgrid()
        Call kosongkanfield()
        Call matikanfield()
        Call itemcombo()
        Call simpandata()
    End Sub

    Private Sub Btntambah_Click(sender As Object, e As EventArgs) Handles btntambah.Click
        Call hidupkanfield()
        Call kosongkanfield()
        txtidtransaksi.Focus()
    End Sub

    Private Sub Cbkodebarang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbkodebarang.SelectedIndexChanged
        Call Koneksi()
        CMD = New OleDb.OleDbCommand("SELECT * FROM tbbarang WHERE kodebarang='" & cbkodebarang.Text & "'", CONN)
        DR = CMD.ExecuteReader
        DR.Read()
        txtnamabarang.Text = DR.Item(1)
        txtpersediaan.Text = DR.Item(2)
        txtjumlah.Focus()
    End Sub


    Private Sub Btnsimpan_Click(sender As Object, e As EventArgs) Handles btnsimpan.Click
        If txtidtransaksi.Text = "" Or datetgl.Text = "" Or cbkodebarang.Text = "" Or txtnamabarang.Text = "" Or txtpersediaan.Text = "" Or txtjumlah.Text = "" Then
            MsgBox("Data Belum Lengkap")
            Exit Sub
        Else
            Call Koneksi()
            CMD = New OleDb.OleDbCommand("SELECT * FROM tbbarangkeluar WHERE kodebarangkeluar='" & txtidtransaksi.Text & "'", CONN)
            DR = CMD.ExecuteReader
            DR.Read()
            If Not DR.HasRows Then
                Dim simpan As String
                simpan = "INSERT INTO tbbarangkeluar VALUES ('" & txtidtransaksi.Text & "','" & datetgl.Text & "','" & cbkodebarang.Text & "', '" & txtjumlah.Text & "')"
                CMD = New OleDb.OleDbCommand(simpan, CONN)
                CMD.ExecuteNonQuery()

                Call Koneksi()
                txtpersediaan.Text = Val(txtpersediaan.Text) - Val(txtjumlah.Text)
                CMD = New OleDb.OleDbCommand("UPDATE tbbarang SET persediaan='" & txtpersediaan.Text & "' WHERE kodebarang='" & cbkodebarang.Text & "'", CONN)
                CMD.ExecuteNonQuery()
                MsgBox("Persediaan Telah Berkurang")
            Else
                MsgBox("ID Transaksi Sudah Ada")
            End If
            Call RefreshGrid()
            Call tampilgrid()
            Call kosongkanfield()
            Call matikanfield()
        End If
    End Sub

    Private Sub Btnbatal_Click(sender As Object, e As EventArgs) Handles btnbatal.Click
        Call kosongkanfield()
        Call matikanfield()
    End Sub

    Private Sub Btnubah_Click(sender As Object, e As EventArgs) Handles btnubah.Click
        If txtidtransaksi.Text = "" Then
            MsgBox("Data yang Akan Diubah Belum Ada")
            Exit Sub
        Else
            Call Koneksi()
            CMD = New OleDb.OleDbCommand("UPDATE tbbarangkeluar SET tanggal='" & datetgl.Text & "', kodebarang='" & cbkodebarang.Text & "', jumlah='" & txtjumlah2.Text & "' WHERE kodebarangkeluar ='" & txtidtransaksi.Text & "'", CONN)
            CMD.ExecuteNonQuery()
            txtpersediaan.Text = Val(txtpersediaan.Text) - Val(txtjumlah2.Text) + Val(txtjumlah.Text)
            CMD = New OleDb.OleDbCommand("UPDATE tbbarang SET persediaan='" & txtpersediaan.Text & "' WHERE kodebarang='" & cbkodebarang.Text & "'", CONN)
            CMD.ExecuteNonQuery()
            MsgBox("Data Berhasil Diubah")
        End If
        Call matikanfield()
        Call kosongkanfield()
        Call tampilgrid()
    End Sub

    Private Sub Btnhapus_Click(sender As Object, e As EventArgs) Handles btnhapus.Click
        If txtidtransaksi.Text = "" Then
            MsgBox("Data Yang Akan Dihapus Belum Ada")
            Exit Sub
        Else
            If MessageBox.Show("Yakin Menghapus Data Ini?", "Konfirmasi", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                txtpersediaan.Text = Val(txtpersediaan.Text) - Val(txtjumlah.Text)
                CMD = New OleDb.OleDbCommand("UPDATE tbbarang SET persediaan='" & txtpersediaan.Text & "' WHERE kodebarang='" & cbkodebarang.Text & "'", CONN)
                CMD.ExecuteNonQuery()
                Call Koneksi()
                CMD = New OleDb.OleDbCommand("DELETE FROM tbbarangkeluar WHERE kodebarangkeluar='" & txtidtransaksi.Text & "'", CONN)
                CMD.ExecuteNonQuery()
                MsgBox("Data Berhasil Dihapus")
                Call matikanfield()
                Call kosongkanfield()
                Call tampilgrid()
            End If
        End If
    End Sub

    Private Sub Txtpencarian_TextChanged(sender As Object, e As EventArgs) Handles txtpencarian.TextChanged
        Call Koneksi()
        CMD = New OleDb.OleDbCommand("SELECT * FROM tbbarangkeluar WHERE kodebarangkeluar LIKE '%" & txtpencarian.Text & "%'", CONN)
        DR = CMD.ExecuteReader
        DR.Read()
        If DR.HasRows Then
            Call Koneksi()
            DA = New OleDb.OleDbDataAdapter("SELECT * FROM tbbarangkeluar WHERE kodebarangkeluar LIKE '%" & txtpencarian.Text & "%'", CONN)
            DS = New DataSet
            DA.Fill(DS)
            Grid2.DataSource = DS.Tables(0)
        Else
            MsgBox("Data Tidak DItemukan")
        End If
    End Sub

    Private Sub Btnkeluar_Click(sender As Object, e As EventArgs) Handles btnkeluar.Click
        Menu_Utama.Show()
        Me.Close()
    End Sub

    Private Sub Grid2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles Grid2.CellContentClick

    End Sub

    Private Sub Grid2_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Grid2.CellMouseClick
        On Error Resume Next
        txtidtransaksi.Text = Grid2.Rows(e.RowIndex).Cells(0).Value
        datetgl.Text = Grid2.Rows(e.RowIndex).Cells(1).Value
        cbkodebarang.Text = Grid2.Rows(e.RowIndex).Cells(2).Value
        txtjumlah.Text = Grid2.Rows(e.RowIndex).Cells(3).Value
        txtjumlah2.Text = txtjumlah.Text
        Call hidupkanfield()
        txtidtransaksi.Enabled = False
    End Sub
End Class