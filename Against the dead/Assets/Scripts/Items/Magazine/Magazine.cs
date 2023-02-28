using System;
using System.Collections.Generic;
using Items.Ammunition;
using Palmmedia.ReportGenerator.Core.Plugin;

namespace Items.Magazine
{

    public class Magazine : Item
    {
        private List<Ammunition.Ammunition> _ammunitions;
        public int _size { get; }

        public int ammoLeft { get; protected set; }

        public Magazine(int size, Caliber caliber)
        {
            _ammunitions = new List<Ammunition.Ammunition>(size);
            _size = size;
            ammoLeft = 0;
        }

        public void Reload(int nbAmmo)
        {
            throw new NotImplementedException();
        }
    }
}