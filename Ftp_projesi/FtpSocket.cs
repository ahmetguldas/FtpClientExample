using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Ftp_projesi
{
    class FtpSoket
    {
        /*
            Baglanti
            Sunucu Adresi
            Istemci Adresi
        */

        Socket _Soket;
        IPEndPoint _IPAdres;

        public FtpSoket(IPEndPoint IPAdres)
        {
            _IPAdres = IPAdres;
        }

        private void SoketOlustur()
        {
            _Soket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void SoketSil()
        {
            if(_Soket != null)
            {
                GC.SuppressFinalize(_Soket);
                _Soket.Dispose();
            }

            _Soket = null;
        }
       
        public Boolean Baglan()
        {
            Boolean _Baglandi = false;

            try
            {
                if (_Soket == null)
                    SoketOlustur();

                if (!_Soket.Connected)
                {
                    IAsyncResult _Sonuc = _Soket.BeginConnect(_IPAdres, null, null);
                    Boolean _Durum = _Sonuc.AsyncWaitHandle.WaitOne();

                    if (_Durum)
                    {
                        _Soket.EndConnect(_Sonuc);
                        _Baglandi = true;
                    }
                }
            }
            catch(SocketException _SoketHatasi)
            {

            }

            return _Baglandi;
        }

        public Boolean BaglantiKes()
        {
            Boolean _BaglantiKesildi = false;

            try
            {
                if(_Soket != null)
                {
                    if(_Soket.Connected)
                    {
                        IAsyncResult _Sonuc = _Soket.BeginDisconnect(true, null, null);
                        Boolean _Durum = _Sonuc.AsyncWaitHandle.WaitOne();

                        if(_Durum)
                        {
                            _Soket.EndDisconnect(_Sonuc);
                            _BaglantiKesildi = true;

                            SoketSil();
                        }
                    }
                }
            }
            catch(SocketException _SoketHatasi)
            {
               
            }

            return _BaglantiKesildi;
        }

        public Int32 VeriGonder(String Veri, Boolean FtpKomutuMu = false)
        {
            Int32 _Gonderilen = -1;

            try
            {
                if(_Soket != null)
                {
                    if (_Soket.Connected)
                    {
                        if (Veri.Length > 0)
                        {
                            if (FtpKomutuMu)
                                Veri += Environment.NewLine;

                            Byte[] _VeriBayt = ASCIIEncoding.ASCII.GetBytes(Veri);

                            IAsyncResult _Sonuc = _Soket.BeginSend(_VeriBayt, 0, _VeriBayt.Length, SocketFlags.None, null, null);
                            Boolean _Durum = _Sonuc.AsyncWaitHandle.WaitOne();

                            if (_Durum)
                            {
                                _Gonderilen = _Soket.EndSend(_Sonuc);
                            }
                        }
                    }
                    else
                        BaglantiKes();
                }
            }
            catch(SocketException _SoketHatasi)
            {
                
            }

            return _Gonderilen;
        }

        public String VeriAl(Boolean FTPKomutuMu = false)
        {
            String _Veri = "";

            try
            {
                if(_Soket!= null)
                {
                    if (_Soket.Connected)
                    {
                        Int32 _TamponBoyutu = 1024;
                        Int32 _Alinan = 0;
                        IAsyncResult _Sonuc;
                        Boolean _Durum;

                        if (FTPKomutuMu)
                            _TamponBoyutu = 1;

                        Byte[] _VeriBayt = new Byte[_TamponBoyutu];

                        while (_Soket.Available > 0)
                        {
                            _Sonuc = _Soket.BeginReceive(_VeriBayt, 0, _VeriBayt.Length, SocketFlags.None, null, null);
                            _Durum = _Sonuc.AsyncWaitHandle.WaitOne();

                            if (_Durum)
                            {
                                _Alinan = _Soket.EndReceive(_Sonuc);
                                _Veri += Encoding.ASCII.GetString(_VeriBayt, 0, _Alinan);

                                if (FTPKomutuMu)
                                {
                                    if (_Veri.EndsWith(Environment.NewLine))
                                    {
                                        _Veri = _Veri.Trim();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                        BaglantiKes();
                }
            }
            catch(SocketException _SoketHatasi)
            {

            }

            return _Veri;
        }
    }
}
