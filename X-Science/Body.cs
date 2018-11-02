using System.Linq;
using UnityEngine;


namespace ScienceChecklist{
	public sealed class Body{
		private string[] _biomes;
		private bool _hasBiomes;
		private bool _hasSurface;
		private bool _reached;

		private bool _isPlanet;
		private bool _isStar;
		private bool _isMoon;
		private bool _isGasGiant; // No surface but isn't a star

		private CelestialBody _parent; // Note: flightGlobalsIndex or null for the sun

		private string _type;
		private CelestialBody _celestialBody;

		public string[] Biomes				{get {return _biomes;}}
		public bool HasBiomes				{get {return _hasBiomes;}}
		public bool HasAtmosphere			{get {return _celestialBody.atmosphere;}}
		public bool HasOxygen				{get {return _celestialBody.atmosphereContainsOxygen;}}
		public bool HasOcean				{get {return _celestialBody.ocean;}}
		public bool HasSurface				{get {return _hasSurface;}}
		public bool IsHome					{get {return _celestialBody.isHomeWorld;}}
		public bool Reached					{get {return _reached;}}
		public bool IsPlanet				{get {return _isPlanet;}}
		public bool IsStar					{get {return _isStar;}}
		public bool IsGasGiant				{get {return _isGasGiant;}}
		public bool IsMoon					{get {return _isMoon;}}
		public string Type					{get {return _type;}}
		public string Name					{get {return _celestialBody.name;}}
		public CelestialBody Parent			{get {return _parent;}}
		public CelestialBody CelestialBody	{get {return _celestialBody;}}

		// This could fail if some mode changes celestial bodies on the fly
		// Just don't want to stick too much stuff into Update()
		public Body(CelestialBody Body)
		{
			_celestialBody = Body;

			// Biomes
			_hasBiomes = false;
			if(_celestialBody.BiomeMap != null)
				_biomes = _celestialBody.BiomeMap.Attributes.Select(y => y.name).ToArray();
			else
				_biomes = new string[0];
			if(_biomes.Length > 0)
				_hasBiomes = true;

			// Surface
			_hasSurface = _celestialBody.pqsController != null;

			// Star detection
			_isStar = Sun.Instance.sun == _celestialBody;

			// GasGiant detection
			_isGasGiant = !_isStar && !_hasSurface;

			// Moon detection + Parent
			_parent = null; // No parent - a star
			_isPlanet = _isMoon = false;
			if(_celestialBody.orbit != null && _celestialBody.orbit.referenceBody != null){  // Otherwise it is the sun
				_parent = _celestialBody.orbit.referenceBody;
				if(_celestialBody.orbit.referenceBody == Sun.Instance.sun)
					_isPlanet = true;
				else
					_isMoon = true; // A moon - parent isn't the sun
			}

			// Type
			_type = FigureOutType();

			// Progress tracking changes
			Update();
		}

		// Todo: Move to ctor
		private string FigureOutType()
		{
			if(_isGasGiant)
				return "Gas Giant";
			if(_isStar)
				return "Star";
			if(_isPlanet)
				return "Planet";
			if(_isMoon)
				return "Moon";
			return "Unknown";
		}

		// Todo: Have to check if this function is still needed. ATM, keep 'IsReached' logic here.
		public void Update(){
			// Consider home as always reached. For other bodies, check the progressTree.flyBy.IsReached value.
			if(IsHome)
				_reached = true;
			else
				_reached = _celestialBody.progressTree.flyBy.IsReached;
		}
	}
}
