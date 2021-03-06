using System;
using System.Collections;
using System.Collections.Generic;
using Tatacoa;

namespace Atoms 
{
	public abstract partial class Chain<A> : Atom, Monad<A>
	{
		public Atom Bind (Func<A, Atom> f)
		{
			return new ChainJoinBond<A> (this.copy as Chain<A>, new Bind<A> (f));	
		}

		public Atom Bind (Bond<A> bond)
		{
			return new ChainJoinBond<A> (this.copy as Chain<A>, bond);	
		}

		public Chain<B> Bind<B> (Func<A,Chain<B>> f)
		{
			return new ChainJoinBond<A,B> (this.copy as Chain<A>, new Bind<A,B> (f));
		}

		public Chain<B> Bind<B> (Bond<A,B> bond)
		{
			return new ChainJoinBond<A,B> (this.copy as Chain<A>, bond.copy as Bond<A,B>);
		}

		public Chain<B> Bind<B> (Bind<A,B> bind)
		{
			return this.Bind ((Bond<A,B>)bind);	
		}

		public Chain<B> Map<B> (Map<A,B> map)
		{
			return this.Bind (map);	
		}

		public Chain<B> Map<B> (Func<A, B> f)
		{
			return new ChainJoinBond<A,B> (this, Map<A,B>._ (f));
		}

		public Chain<A> Map (Action<A> f)
		{
			return Map (f.ToFunc ());
		}

		Monad<B> Monad<A>.Bind<B> (Func<A, Monad<B>> f)
		{
			return Bind (f as Func<A,Chain<B>>);
		}

		Functor<A> Applicative<A>.Pure (A value)
		{
			return Atoms.Do._ (() => value);
		}

		Functor<B> Functor<A>.FMap<B> (Func<A, B> f)
		{
			return Map (f);
		}

		Functor<A> Functor<A>.XMap (Func<Exception, Exception> f)
		{
			throw new NotImplementedException ();
		}

		public Map<A,B> MakeMap<B> (Func<A,B> f)
		{
			return new Map<A, B> (f);	
		}

		public Map<A,A> MakeMap (Action<A> f)
		{
			return MakeMap (f.ToFunc());
		}

		public Bind<A,B> MakeBind<B> (Func<A,Chain<B>> f)
		{
			return new Bind<A, B> (f);	
		}

		public Chain<B> Then<B> (Func<A,B> f)
		{
			return Map (f);
		}

		public Chain<A> Then (Action<A> f)
		{
			return Map (f);
		}

		public Chain<A> Then (Action f)
		{
			return Map (f.ToFunc<A>());
		}

		public Chain<B> Then<B> (Func<A,Chain<B>> f)
		{
			return Bind (f);
		}

		public AtomParallelChain<A> Parallel (Atom other) 
		{
			return AtomParallelChain<A>._ (other, this);
		}

		public ChainParallelChain<A,B> Parallel<B> (Chain<B> other) 
		{
			return ChainParallelChain<A,B>._ (this, other);
		}

		public static Chain<A> operator + (Atom a, Chain<A> b)
		{
			return a.Then (b);
		}

		public static Chain<A> operator * (int n, Chain<A> chain) 
		{
			return chain.Replicate (n);
		}
		
		public static Chain<A> operator * (Chain<A> chain, int n) 
		{
			return n * chain;
		}
	}

	public abstract class BoundedChain<A> : BoundedAtom, Monad<A>
	{
		public Monad<B> Bind<B> (Func<A, Monad<B>> f)
		{
			throw new NotImplementedException ();
		}

		public Functor<A> Pure (A value)
		{
			throw new NotImplementedException ();
		}

		public Functor<B> FMap<B> (Func<A, B> f)
		{
			throw new NotImplementedException ();
		}

		public Functor<A> XMap (Func<Exception, Exception> f)
		{
			throw new NotImplementedException ();
		}

		public static Chain<A> operator + (Atom a, BoundedChain<A> b)
		{
			return new AtomJoinBoundChain<A> (a.copy as Atom, b.copy as BoundedChain<A>);
		}
	}
}
//using System;
//using System.Collections;
//
//namespace Atoms {
//	public abstract class Chain<A> : Atom, Monad<A> {
//
//		public Chain () {}
//		public Chain (Quantum prev, Quantum next) : base (prev, next) {}
//
//		public Map<A,B> MakeMap<B> (Func<A,B> f)
//		{
//			return Map<A,B>._ (f);
//		}
//
//		public Map<A,A> MakeMap (Action<A> f)
//		{
//			return Map<A,A>._ (f.ToFunc());
//		}
//
//		public Bind<A,B> MakeBind<B> (Func<A,Chain<B>> f) {
//			return Atoms.Bind._ (f);
//		}
//
//		public Chain<B> FMap<B> (Func<A,B> f)
//		{
//			return new Map<A,B> (f, this).MakeChain();
//		} 
//
//		public Chain<A> FMap (Action<A> f)
//		{
//			return FMap (f.ToFunc ());
//		}
//		public Chain<A> Pure (A value)
//		{
//			return Do._ (() => value);
//		}
//
//		public Chain<B> Bind<B> (Func<A,Chain<B>> f)
//		{
//			return new Bind<A,B> (f, this).MakeChain();
//		}
//
//		public Chain<A> XMap (Func<Exception,Exception> f){
//			throw new NotImplementedException ();
//			return default (Chain<A>);
//		} 
//		public Chain<A> XMap (Action<Exception> f) {
//			return XMap (f.ToFunc ());
//		}
//
//		public override Atom copyAtom {
//			get {
//				return copyChain;
//			}
//		}
//
//		public abstract Chain<A> copyChain { get;}
//
//		public Chain<A> MakeChain () {
//			return this;
//		}
//
//		Monad<B> Monad<A>.Bind<B> (Func<A, Monad<B>> f)
//		{
//			return Bind (f as Func<A,Chain<B>>);
//		}
//
//		Functor<A> Applicative<A>.Pure (A value)
//		{
//			return Pure (value);
//		}
//
//		Functor<B> Functor<A>.FMap<B> (Func<A, B> f)
//		{
//			return FMap (f);
//		}
//
//		Functor<A> Functor<A>.XMap (Func<Exception, Exception> f)
//		{
//			return XMap (f);
//		}
//
//		public static Chain<A> operator + (Atom a, Chain<A> b)
//		{
//			return a.Then (b);
//		}
//
//	}
//	public interface IChain<A> : IEnumerable {
//		Chain<A> MakeChain ();
//	}
//}